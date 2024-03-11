using System.Collections.Immutable;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Tasks.Dto;
using EventsManager.Domain.Producer;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;
using SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

namespace TaskTracker.Domain.Tasks.Management;

internal class TasksManager(ITasksRepository repository, IEmployeesManager employeesManager, IEventProducer producer, EventsFactory taskEventsFactory)
    : ITasksManager
{
    private readonly Random _rnd = new();

    public Task<IReadOnlyCollection<TaskDto>> ListOpen() => repository.ListOpen();

    public Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime) =>
        repository.ListByFinishDate(dateTime);

    public async Task Close(TaskId id)
    {
        var editDto = new TaskManagementDto
        {
            Id = id,
            IsClosed = true
        };
        await repository.Update(editDto);

        var task = await repository.GetById(id);
        var @event = await taskEventsFactory.CreateTaskStatusChangedEvent(task.PublicId, task.DeveloperId.Value,
            ChangedTaskType.Closed);
        await producer.Produce("task-workflow", ChangedTaskType.Closed.ToString(), @event);
    }

    public async Task Create(string jiraId, string title, string description)
    {
        var dto = new CreateTaskDto()
        {
            Title = title,
            Description = description,
            DeveloperId = await GetDeveloperForTask()
        };
        var id = await repository.Create(dto);
        var task = await repository.GetById(id);

        var eventV1 = await taskEventsFactory.CreateTaskCreatedEvent(task.PublicId, task.Title);
        var eventV2 = await taskEventsFactory.CreateTaskCreatedEvent(task.PublicId, task.Title);
        
        await producer.Produce("task-streaming", TaskCreatedEventVersion.V1.ToString(), eventV1);
        await producer.Produce("task-streaming", TaskCreatedEventVersion.V2.ToString(), eventV2);
    }

    public async Task Reassign()
    {
        var openTasks = await repository.ListOpen();
        var developers = (await employeesManager.ListAllDevelopers()).ToImmutableArray();

        await repository.Update(openTasks.Select(t => new TaskManagementDto()
        {
            Id = t.Id,
            DeveloperId = GetDeveloperForTask(developers)
        }).ToArray());
        
        // TODO: add batching, add 2 types of events supporting
        foreach (var task in await repository.ListOpen())
        {
            var @event = await taskEventsFactory.CreateTaskStatusChangedEvent(task.PublicId, task.DeveloperId.Value,
                ChangedTaskType.Reassigned);
            await producer.Produce("task-workflow",
                ChangedTaskType.Reassigned.ToString(), @event);
        }
    }

    private EmployeeId GetDeveloperForTask(IReadOnlyList<EmployeeId> developers)
    {
        return developers[_rnd.Next(developers.Count)];
    }

    private async Task<EmployeeId> GetDeveloperForTask()
    {
        var developers = (await employeesManager.ListAllDevelopers()).ToImmutableArray();
        return developers[_rnd.Next(developers.Count())];
    }
}
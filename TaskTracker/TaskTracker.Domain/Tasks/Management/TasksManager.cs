using System.Collections.Immutable;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Tasks.Dto;
using EventsManager.Domain.Producer;

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
        var editDto = new TaskManagementDto()
        {
            Id = id,
            IsClosed = true
        };
        await repository.Update(editDto);

        var task = await repository.GetById(id);
        await producer.Produce("task-workflow", TaskStatus.Closed.ToString(),
            taskEventsFactory.CreateTaskStatusChangedEvent(task.PublicId, task.DeveloperId.Value, TaskStatus.Closed));
    }

    public async Task Create(string description)
    {
        var dto = new CreateTaskDto()
        {
            Description = description,
            DeveloperId = await GetDeveloperForTask()
        };
        var id = await repository.Create(dto);
        var task = await repository.GetById(id);
        
        await producer.Produce("task-workflow", TaskStatus.Closed.ToString(), taskEventsFactory.CreateTaskCreatedEvent(task.PublicId));
    }

    public async Task Reassign()
    {
        var openTasks = await repository.ListOpen();
        var developers = (await employeesManager.ListAllDevelopers()).ToImmutableArray();

        await repository.Update(openTasks.Select(t => new TaskManagementDto()
        {
            DeveloperId = GetDeveloperForTask(developers)
        }).ToArray());
        
        // TODO: add batching, add 2 types of events supporting
        foreach (var task in await repository.ListOpen())
        {
            await producer.Produce("task-workflow", TaskStatus.Reassigned.ToString(),
                taskEventsFactory.CreateTaskStatusChangedEvent(task.PublicId, task.DeveloperId.Value, TaskStatus.Reassigned));
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
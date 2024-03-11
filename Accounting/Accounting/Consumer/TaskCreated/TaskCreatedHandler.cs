using Accounting.Domain.Tasks;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

namespace Accounting.Consumer.TaskCreated;

public class TaskCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<TaskCreatedHandler> logger) : IEventHandler<TaskCreatedEvent_V1>
{
    public async Task Handle(TaskCreatedEvent_V1 taskCreated)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var tasksRepository = scope.ServiceProvider.GetRequiredService<ITasksRepository>();
        await tasksRepository.Add(new TaskId(taskCreated.TaskId));
        logger.LogInformation($"Task {taskCreated.TaskId} was added.");
    }

    public string TopicName => "task-streaming";
}
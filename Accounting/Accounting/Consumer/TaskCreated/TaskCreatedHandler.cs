using Accounting.Domain.Tasks;

namespace Accounting.Consumer.TaskCreated;

public class TaskCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger logger) : IEventHandler<TaskCreatedEvent>
{
    public async Task Handle(TaskCreatedEvent taskCreated)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var tasksRepository = scope.ServiceProvider.GetRequiredService<ITasksRepository>();
        await tasksRepository.Add(new TaskId(taskCreated.TaskId));
        logger.LogInformation($"Task {taskCreated.TaskId} was added.");
    }

    public string TopicName => "tasks-streaming";
}
using Accounting.Domain.Tasks;
using Confluent.Kafka;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

namespace Accounting.Consumer.TaskCreated;

public class TaskCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<TaskCreatedHandler> logger) : IEventHandler
{
    public async Task Handle(Message<string, string> message)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<EventsFactory>();

        var versionHeader = message.Headers.FirstOrDefault(h => h.Key.Equals("Version"));
        var taskCreated = await factory.CreateTaskCreatedEvent(message.Value, GetVersion(versionHeader));
        
        var tasksRepository = scope.ServiceProvider.GetRequiredService<ITasksRepository>();
        await tasksRepository.Add(new TaskId(taskCreated.TaskId));
        logger.LogInformation($"Task {taskCreated.TaskId} was added.");
    }

    public string TopicName => "task-streaming";

    private TaskCreatedEventVersion GetVersion(IHeader? header)
    {
        if (header == null)
        {
            return TaskCreatedEventVersion.V1;
        }
        var version = System.Text.Encoding.UTF8.GetString(header.GetValueBytes());
        var isValid = Enum.TryParse<TaskCreatedEventVersion>(version, out var value);
        return isValid ? value : TaskCreatedEventVersion.V1;
    }
}
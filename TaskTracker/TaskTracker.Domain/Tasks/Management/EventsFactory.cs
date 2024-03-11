using System.Text.Json;
using System.Text.Json.Serialization;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;
using SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

namespace TaskTracker.Domain.Tasks.Management;

public class EventsFactory(
    TaskStatusChangedEventSchemaRegistry taskStatusChangedEventSchemaRegistry,
    TaskCreatedEventSchemaRegistry taskCreatedEventSchemaRegistry)
{
    private const string LastSupportedVersion = "V1";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    public async Task<string> CreateTaskCreatedEvent(Guid id, string title)
    {
        var producedEvent = new TaskCreatedEvent_V1
        {
            TaskId = id,
            Title = title
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        await taskCreatedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);

        return serialized;
    }
    public async Task<string> CreateTaskCreatedEvent(Guid id, string title, string jiraId)
    {
        var producedEvent = new TaskCreatedEvent_V2
        {
            TaskId = id,
            Title = title,
            JiraId = jiraId
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);

        await taskCreatedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);

        return serialized;
    }

    public async Task<string> CreateTaskStatusChangedEvent(Guid taskId, Guid developerId, ChangedTaskType type)
    {
        var producedEvent = new TaskStatusChangedEvent_V1
        {
            TaskId = taskId,
            DeveloperId = developerId,
            Type = type,
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        await taskStatusChangedEventSchemaRegistry.Validate(serialized, LastSupportedVersion);
        return serialized;
    }
}
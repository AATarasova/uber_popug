using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema;
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

    public string CreateTaskCreatedEvent(Guid id)
    {
        var producedEvent = new TaskCreatedEvent_V1
        {
            TaskId = id,
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        var jsonSchema = taskCreatedEventSchemaRegistry.GetSchemaByVersion(LastSupportedVersion).Result;
        var validationErrors = JsonSchema.FromJsonAsync(jsonSchema).Result.Validate(serialized);

        if (validationErrors.Any())
        {
            throw new InvalidCastException($"{nameof(TaskCreatedEvent_V1)} not match to {LastSupportedVersion}");
        }

        return serialized;
    }

    public string CreateTaskStatusChangedEvent(Guid taskId, Guid developerId, SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent.TaskStatus status)
    {
        var producedEvent = new TaskStatusChangedEvent_V1
        {
            TaskId = taskId,
            DeveloperId = developerId,
            Status = status
        };
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        var jsonSchema = taskStatusChangedEventSchemaRegistry.GetSchemaByVersion(LastSupportedVersion).Result;
        var validationErrors = JsonSchema.FromJsonAsync(jsonSchema).Result.Validate(serialized);

        if (validationErrors.Any())
        {
            throw new InvalidCastException($"{nameof(TaskCreatedEvent_V1)} not match to {LastSupportedVersion}");
        }

        return serialized;
    }
}
namespace SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

public class TaskCreatedEvent_V1
{
    public Guid TaskId { get; init; }
    
    public EventMeta<TaskCreatedEventVersion> EventMeta { get; } = new()
    {
        Version = TaskCreatedEventVersion.V1
    };
}
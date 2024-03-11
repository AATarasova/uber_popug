namespace SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

public class TaskStatusChangedEvent_V1
{
    public Guid TaskId { get; init; }
    public Guid DeveloperId { get; init; }
    public ChangedTaskType Type { get; init; }
    
    public EventMeta<TaskStatusChangedEventVersion> EventMeta { get; } = new()
    {
        Version = TaskStatusChangedEventVersion.V1
    };
}

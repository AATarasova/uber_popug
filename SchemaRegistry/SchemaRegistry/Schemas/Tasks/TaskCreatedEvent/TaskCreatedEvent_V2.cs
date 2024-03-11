namespace SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;

public class TaskCreatedEvent_V2
{
    public Guid TaskId { get; init; }
    public string Title { get; init; } = null!;
    public string JiraId { get; init; } = null!;

    public EventMeta<TaskCreatedEventVersion> EventMeta { get; } = new()
    {
        Version = TaskCreatedEventVersion.V2
    };
}
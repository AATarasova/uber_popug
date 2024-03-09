namespace Accounting.Consumer.TaskAssigned;

public class TaskStatusChangedEvent
{
    public Guid TaskId { get; init; }
    public Guid DeveloperId { get; init; }
    public TaskStatus Status { get; init; }
}
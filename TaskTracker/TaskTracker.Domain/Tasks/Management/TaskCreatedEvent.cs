namespace TaskTracker.Domain.Tasks.Management;

public class TaskCreatedEvent
{
    public Guid TaskId { get; init; }
}
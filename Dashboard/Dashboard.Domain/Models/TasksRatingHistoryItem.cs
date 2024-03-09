namespace Dashboard.Domain.Models;

public class TasksRatingHistoryItem
{
    public int Id { get; init; }
    public Guid TaskId { get; init; }
    public ulong Cost { get; init; }
    public DateTime Date { get; init; }
}
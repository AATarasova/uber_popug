namespace Dashboard.Domain.Tasks;

public class TasksRatingHistoryItem
{
    public DateTime Date { get; init; }
    public Guid TaskId { get; init; }
    public ulong Cost { get; init; }
}
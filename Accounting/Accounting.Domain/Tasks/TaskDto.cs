namespace Accounting.Domain.Tasks;

public class TaskDto
{
    public TaskId TaskId { get; init; }
    public ulong AssignmentPrice { get; init; }
    public ulong CompletionPrice { get; init; }
}
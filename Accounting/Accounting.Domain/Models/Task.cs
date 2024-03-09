namespace Accounting.Domain.Models;

public class Task
{
    public Guid TaskId { get; init; }
    public ulong AssignmentPrice { get; init; }
    public ulong CompletionPrice { get; init; }
}
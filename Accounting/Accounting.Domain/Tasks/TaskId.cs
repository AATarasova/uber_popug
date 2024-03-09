namespace Accounting.Domain.Tasks;

public struct TaskId(Guid value)
{
    public Guid Value { get; init; } = value;
}

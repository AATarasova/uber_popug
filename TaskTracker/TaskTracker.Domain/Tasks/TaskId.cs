namespace TaskTracker.Domain.Tasks;

public struct TaskId
{
    public TaskId(int value)
    {
        Value = value;
    }

    public int Value { get; init; }
}
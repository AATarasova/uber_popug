namespace TaskTracker.Domain.Employees;

public struct EmployeeId
{
    public EmployeeId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; init; }
}
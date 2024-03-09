namespace Accounting.Domain.Accounts;

public struct EmployeeId(Guid value)
{
    public Guid Value { get; init; } = value;
}

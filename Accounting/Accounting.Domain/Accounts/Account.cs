namespace Accounting.Domain.Accounts;

public class Account
{
    public AccountId Id { get; init; }
    public EmployeeId StaffMemberId { get; init; }
    public long Balance { get; init; }
}
namespace Accounting.Domain.Models;

public class Account
{
    public int Id { get; init; }
    public Guid EmployeeId { get; init; }
    public long Balance { get; set; }
}
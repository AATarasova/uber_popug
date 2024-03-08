using Accounting.Domain.Accounts;

namespace Accounting.Consumer.EmployeeCreated;

public class EmployeeCreatedEvent
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}
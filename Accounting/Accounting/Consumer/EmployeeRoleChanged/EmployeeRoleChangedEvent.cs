using Accounting.Domain.Accounts;

namespace Accounting.Consumer.EmployeeRoleChanged;

public class EmployeeRoleChangedEvent
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}
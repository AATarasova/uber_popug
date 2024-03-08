using Auth.Domain.Roles;

namespace Auth.Domain.Users.Events;

public class EmployeeRoleChangedEvent
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}
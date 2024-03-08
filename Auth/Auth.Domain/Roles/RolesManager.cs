using Auth.Domain.Users;

namespace Auth.Domain.Roles;

public interface IRolesManager
{
    Task ChangeRole(UserId userId, Role newRole);
}
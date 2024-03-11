using Auth.Domain.Roles;

namespace Auth.Domain.Users.Management;

public interface IUserManager
{
    Task Create(CreateUserDto user);
    Task UpdateRole(UserId user, Role role);
}
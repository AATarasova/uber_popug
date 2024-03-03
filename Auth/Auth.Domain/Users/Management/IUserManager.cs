using Auth.Domain.Roles;
using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Management;

public interface IUserManager
{
    Task Create(CreateUserDto user);
    Task UpdateRole(UserId user, Role role);
}
using Auth.Domain.Roles;
using Auth.Domain.Users.Events;
using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Management;

public class UserManager(IUserRepository repository, IRolesManager rolesManager, IEventProducer producer) : IUserManager
{
    public async Task Create(CreateUserDto user)
    {
        var id = await repository.Create(user);

        var createdUser = await repository.GetById(id);
        await producer.Produce("employees-streaming", "Created", new EmployeeCreatedEvent
        {
            EmployeeId = createdUser.PublicId,
            Role = createdUser.Role,
        });
    }
    
    public async Task UpdateRole(UserId user, Role role)
    {
        await rolesManager.ChangeRole(user, role);

        var updated = await repository.GetById(user);
        await producer.Produce("employee-role-updates", "RoleUpdated", new EmployeeRoleChangedEvent {
            EmployeeId = updated.PublicId,
            Role = updated.Role,
        });
    }
}
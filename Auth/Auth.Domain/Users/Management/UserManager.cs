using Auth.Domain.Roles;
using Auth.Domain.Users.Events;
using EventsManager.Domain.Producer;

namespace Auth.Domain.Users.Management;

public class UserManager(IUserRepository repository, IRolesManager rolesManager, IEventProducer producer, EventsFactory eventsFactory) : IUserManager
{
    public async Task Create(CreateUserDto user)
    {
        var id = await repository.Create(user);

        var createdUser = await repository.GetById(id);
        var @event = await eventsFactory.CreateEmployeeCreatedEvent(createdUser.PublicId, createdUser.Role);
        await producer.Produce("employees-streaming", "Created", @event);
    }
    
    public async Task UpdateRole(UserId user, Role role)
    {
        await rolesManager.ChangeRole(user, role);

        var updated = await repository.GetById(user);
        var @event = await eventsFactory.CreateEmployeeRoleChanged(updated.PublicId, updated.Role);
        await producer.Produce("employee-role-updates", "RoleUpdated", @event);
    }
}
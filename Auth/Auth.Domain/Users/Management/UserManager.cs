using Auth.Domain.Users.Events;
using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Management;

public class UserManager(IUserRepository repository) : IUserManager
{
    public async Task Create(CreateUserDto user, IEventProducer producer)
    {
        var id = await repository.Create(user);

        var createdUser = await repository.GetById(id);
        await producer.Produce("create_employees", new UserCreatedEvent()
        {
            Id = createdUser.PublicId,
            Role = createdUser.Role
        });
    }
}
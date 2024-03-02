using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Management;

public interface IUserManager
{
    Task Create(CreateUserDto user, IEventProducer producer);
}
using Auth.Domain.Roles;
using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Events;

public class UserCreatedEvent : ProducedEvent
{
    public Role Role { get; init; }
}
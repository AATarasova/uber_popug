using Auth.Domain.Roles;
using EventManager.Domain.Producer;

namespace Auth.Domain.Users.Events;

public class UserChangedEvent : ProducedEvent
{
    public Role Role { get; init; }
    public UserEventType Type { get; init; }
}
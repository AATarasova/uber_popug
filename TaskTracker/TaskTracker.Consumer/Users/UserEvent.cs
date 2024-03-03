using TaskTracker.Domain.Employees;

namespace TaskTracker.Consumer.Users;

public class UserEvent
{
    public Guid Id { get; init; }
    public Role Role { get; init; }
    public UserEventType Type { get; init; }
}
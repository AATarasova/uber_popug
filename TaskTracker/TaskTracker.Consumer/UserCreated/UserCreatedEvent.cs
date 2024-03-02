using TaskTracker.Domain.Employees;

namespace TaskTracker.Consumer.UserCreated;

public class UserCreatedEvent
{
    public Guid Id { get; init; }
    public Role Role { get; init; }
}
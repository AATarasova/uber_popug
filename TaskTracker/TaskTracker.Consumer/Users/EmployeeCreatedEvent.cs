using TaskTracker.Domain.Employees;

namespace TaskTracker.Consumer.Users;

public class EmployeeCreatedEvent
{
    public Guid EmployeeId { get; set; }
    public Role Role { get; init; }
}
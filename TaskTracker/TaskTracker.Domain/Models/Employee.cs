using TaskTracker.Domain.Employees;

namespace TaskTracker.Domain.Models;

internal class Employee
{
    public Guid Id { get; set; }
    public Role Role { get; set; }
}
using TaskTracker.Domain.Employees;

namespace TaskTracker.Domain.Tasks.Dto;

public class CreateTaskDto
{
    public EmployeeId DeveloperId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
}
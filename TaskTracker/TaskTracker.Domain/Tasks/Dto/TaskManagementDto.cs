using TaskTracker.Domain.Employees;

namespace TaskTracker.Domain.Tasks.Dto;

public class TaskManagementDto
{
    public TaskId Id { get; set; }
    public EmployeeId? DeveloperId { get; set; }
    public string? Title { get; set; }
    public string? JiraId { get; set; }
    public string? Description { get; set; }
    public bool? IsClosed { get; set; }
}
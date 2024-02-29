using TaskTracker.Domain.Employees;

namespace TaskTracker.Domain.Tasks.Dto;

public class TaskDto
{
    public TaskId Id { get; set; }
    public Guid PublicId { get; set; }
    public EmployeeId DeveloperId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public TaskStatus Status => FinishedDate.HasValue ? TaskStatus.Finished : TaskStatus.Open;
}
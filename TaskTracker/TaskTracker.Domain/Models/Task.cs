namespace TaskTracker.Domain.Models;

internal class Task
{
    public int Id { get; set; }
    public Guid PublicId { get; set; }
    public Guid DeveloperId { get; set; }
    public string Description { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
}
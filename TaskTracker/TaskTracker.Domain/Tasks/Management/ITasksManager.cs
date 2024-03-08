using TaskTracker.Domain.Tasks.Dto;

namespace TaskTracker.Domain.Tasks.Management;

public interface ITasksManager
{
    Task<IReadOnlyCollection<TaskDto>> ListOpen();
    Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime);
    Task Close(TaskId id);
    Task Create(string description);
    Task Reassign();
}
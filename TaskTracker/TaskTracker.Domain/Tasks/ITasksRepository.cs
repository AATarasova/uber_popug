using TaskTracker.Domain.Tasks.Dto;

namespace TaskTracker.Domain.Tasks;

internal interface ITasksRepository
{
    Task<IReadOnlyCollection<TaskDto>> ListOpen();
    Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime);
    Task Create(CreateTaskDto createTask);
    Task Update(TaskManagementDto taskInfo);
}
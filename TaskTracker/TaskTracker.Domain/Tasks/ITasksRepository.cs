using TaskTracker.Domain.Tasks.Dto;

namespace TaskTracker.Domain.Tasks;

internal interface ITasksRepository
{
    Task<IReadOnlyCollection<TaskDto>> ListOpen();
    Task<TaskDto> GetById(TaskId taskId);
    Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime);
    Task<TaskId> Create(CreateTaskDto createTask);
    Task Update(TaskManagementDto taskInfo);
    Task Update(IReadOnlyCollection<TaskManagementDto> taskInfo);
}
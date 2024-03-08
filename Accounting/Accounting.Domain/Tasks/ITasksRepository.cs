namespace Accounting.Domain.Tasks;

internal interface ITasksRepository
{
    Task Add(TaskId taskId);
    Task<TaskDto> GetById(TaskId taskId);
}
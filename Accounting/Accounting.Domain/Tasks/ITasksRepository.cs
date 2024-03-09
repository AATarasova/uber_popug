namespace Accounting.Domain.Tasks;

public interface ITasksRepository
{
    Task Add(TaskId taskId);
    Task<TaskDto> GetById(TaskId taskId);
}
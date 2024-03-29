using TaskTracker.DAL.Context;
using TaskTracker.Domain.Tasks;
using TaskTracker.Domain.Tasks.Dto;
using DbTask = TaskTracker.Domain.Models.Task;

namespace TaskTracker.DAL.Repository;

internal class TasksRepository(TaskTrackerDbContext dbContext) : ITasksRepository
{
    public Task<IReadOnlyCollection<TaskDto>> ListOpen()
    {
        var tasks = dbContext.Tasks
            .Where(t => !t.FinishedDate.HasValue)
            .AsEnumerable()
            .Select(Convert)
            .ToList();
        return Task.FromResult<IReadOnlyCollection<TaskDto>>(tasks);
    }

    public Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime)
    {
        var tasks = dbContext.Tasks
            .Where(t => t.FinishedDate.HasValue && t.FinishedDate == dateTime)
            .AsEnumerable()
            .Select(Convert)
            .ToList();
        return Task.FromResult<IReadOnlyCollection<TaskDto>>(tasks);
    }

    public async Task Create(CreateTaskDto dto)
    {
        var task = new DbTask()
        {
            DeveloperId = dto.DeveloperId.Value,
            Description = dto.Description,
        };
        await dbContext.AddAsync(task);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(TaskManagementDto taskInfo)
    {
        var task = dbContext.Tasks.First(t => t.Id == taskInfo.Id.Value);
        SetFields(task, taskInfo);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(IReadOnlyCollection<TaskManagementDto> updates)
    {
        var dict = updates.ToDictionary(u => u.Id.Value);
        foreach (var task in dbContext.Tasks.Where(t => dict.ContainsKey(t.Id)))
        {    
            SetFields(task, dict[task.Id]);
        }
        await dbContext.SaveChangesAsync();
    }

    private void SetFields(DbTask task, TaskManagementDto updateDto)
    {
        if (updateDto.Description is not null)
        {
            task.Description = updateDto.Description;
        }

        if (updateDto.DeveloperId.HasValue)
        {
            task.DeveloperId = updateDto.DeveloperId.Value.Value;
        }
        if (updateDto.IsClosed.HasValue && updateDto.IsClosed.Value)
        {
            task.FinishedDate = DateTime.Now;
        }
    }

    private TaskDto Convert(DbTask task) =>
        new()
        {
            Id = new TaskId(task.Id),
            PublicId = task.PublicId,
            Description = task.Description,
            CreatedDate = task.CreatedDate,
            FinishedDate = task.FinishedDate
        };
}
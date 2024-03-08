using System.Collections.Immutable;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Tasks.Dto;

namespace TaskTracker.Domain.Tasks.Management;

internal class TasksManager : ITasksManager
{
    private readonly ITasksRepository _repository;
    private readonly IEmployeesManager _employeesManager;

    private readonly Random _rnd;
    
    public TasksManager(ITasksRepository repository, IEmployeesManager employeesManager)
    {
        _repository = repository;
        _employeesManager = employeesManager;
        _rnd = new Random();
    }

    public Task<IReadOnlyCollection<TaskDto>> ListOpen() => _repository.ListOpen();

    public Task<IReadOnlyCollection<TaskDto>> ListByFinishDate(DateTime dateTime) =>
        _repository.ListByFinishDate(dateTime);

    public async Task Close(TaskId id)
    {
        var editDto = new TaskManagementDto()
        {
            Id = id,
            IsClosed = true
        };
        await _repository.Update(editDto);
    }

    public async Task Create(string description)
    {
        var task = new CreateTaskDto()
        {
            Description = description,
            DeveloperId = await GetDeveloperForTask()
        };
        await _repository.Create(task);
    }

    public async Task Reassign()
    {
        var openTasks = await _repository.ListOpen();
        var developers = (await _employeesManager.ListAllDevelopers()).ToImmutableArray();
        foreach (var task in openTasks)
        {
        }
        
        await _repository.Update(openTasks.Select(t => new TaskManagementDto()
        {
            DeveloperId = GetDeveloperForTask(developers)
        }).ToArray());
    }

    private EmployeeId GetDeveloperForTask(IReadOnlyList<EmployeeId> developers)
    {
        return developers[_rnd.Next(developers.Count)];
    }

    private async Task<EmployeeId> GetDeveloperForTask()
    {
        var developers = (await _employeesManager.ListAllDevelopers()).ToImmutableArray();
        return developers[_rnd.Next(developers.Count())];
    }
}
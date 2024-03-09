using Dashboard.Domain.Tasks;

namespace Dashboard.Domain.Company;

public interface ITasksRatingRepository
{
    public Task Add(TasksRatingHistoryItem historyItem);
    public Task<IReadOnlyCollection<TasksRatingHistoryItem>> ListByDates(DateTime startDate, DateTime finishDate);
}
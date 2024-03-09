using Dashboard.DAL.Context;
using Dashboard.Domain.Company;
using Dashboard.Domain.Tasks;
using Entity = Dashboard.Domain.Models.TasksRatingHistoryItem;

namespace Dashboard.DAL.Repositories;

public class TasksRatingRepository(DashboardDbContext dbContext) : ITasksRatingRepository
{
    public async Task Add(TasksRatingHistoryItem historyItem)
    {
        await dbContext.TasksRating.AddAsync(new Entity
        {
            Cost = historyItem.Cost,
            TaskId = historyItem.TaskId,
            Date = historyItem.Date
        });
    }

    public Task<IReadOnlyCollection<TasksRatingHistoryItem>> ListByDates(DateTime startDate, DateTime finishDate)
    {
        var items = dbContext.TasksRating.Where(e => e.Date >= startDate && e.Date <= finishDate);
        var rating = items.Select(i => new TasksRatingHistoryItem
            {
                Cost = i.Cost,
                Date = i.Date,
                TaskId = i.TaskId
            })
            .ToList() as IReadOnlyCollection<TasksRatingHistoryItem>;
        return Task.FromResult(rating);
    }
}
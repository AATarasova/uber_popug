using Dashboard.DAL.Context;
using Dashboard.Domain.Employee;
using Entity = Dashboard.Domain.Models.EmployeesProductivityHistoryItem;

namespace Dashboard.DAL.Repositories;

public class EmployeeProductivityRepository(DashboardDbContext dbContext) : IEmployeeProductivityRepository
{
    public async Task Add(EmployeesProductivityHistoryItem historyItem)
    {
        await dbContext.EmployeesProductivity.AddAsync(new Entity
        {
            Date = historyItem.Date,
            EmployeesNumber = historyItem.EmployeesNumber,
            UnproductiveEmployeesNumber = historyItem.UnproductiveEmployeesNumber
        });
    }

    Task<IReadOnlyCollection<EmployeesProductivityHistoryItem>> IEmployeeProductivityRepository.ListByDates(
        DateTime startDate, DateTime finishDate)
    {
        var items = dbContext.EmployeesProductivity.Where(e => e.Date >= startDate && e.Date <= finishDate);
        var rating = items.Select(i => new EmployeesProductivityHistoryItem
            {
                Date = i.Date,
                EmployeesNumber = i.EmployeesNumber,
                UnproductiveEmployeesNumber = i.UnproductiveEmployeesNumber
            })
            .ToList() as IReadOnlyCollection<EmployeesProductivityHistoryItem>;
        return Task.FromResult(rating);
    }
}
using Dashboard.DAL.Context;
using Dashboard.Domain.Company;
using Entity = Dashboard.Domain.Models.CompanyAccountHistoryItem;

namespace Dashboard.DAL.Repositories;

public class CompanyAccountRepository(DashboardDbContext dbContext) : ICompanyAccountRepository
{
    public async Task Add(CompanyAccountHistoryItem historyItem)
    {        
        await dbContext.CompanyAccount.AddAsync(new Entity
        {
            Date = historyItem.Date,
            Balance = historyItem.Balance
        });
    }

    public Task<IReadOnlyCollection<CompanyAccountHistoryItem>> ListByDates(DateTime startDate, DateTime finishDate)
    {
        var items = dbContext.CompanyAccount.Where(e => e.Date >= startDate && e.Date <= finishDate);
        var rating = items.Select(i => new CompanyAccountHistoryItem
            {
                Date = i.Date,
                Balance = i.Balance
            })
            .ToList() as IReadOnlyCollection<CompanyAccountHistoryItem>;
        return Task.FromResult(rating);
    }
}
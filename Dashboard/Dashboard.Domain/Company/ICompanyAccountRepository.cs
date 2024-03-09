namespace Dashboard.Domain.Company;

public interface ICompanyAccountRepository
{
    public Task Add(CompanyAccountHistoryItem historyItem);
    public Task<IReadOnlyCollection<CompanyAccountHistoryItem>> ListByDates(DateTime startDate, DateTime finishDate);
}
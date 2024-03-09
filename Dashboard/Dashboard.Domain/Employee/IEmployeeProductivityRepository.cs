namespace Dashboard.Domain.Employee;

public interface IEmployeeProductivityRepository
{
    public Task Add(EmployeesProductivityHistoryItem historyItem);
    public Task<IReadOnlyCollection<EmployeesProductivityHistoryItem>> ListByDates(DateTime startDate, DateTime finishDate);
}
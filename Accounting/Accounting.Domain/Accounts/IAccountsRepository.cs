namespace Accounting.Domain.Accounts;

public interface IAccountsRepository
{
    Task Add(EmployeeId employeeId);
    Task Delete(EmployeeId employeeId);
    Task<bool> CheckExists(EmployeeId employeeId);
    Task<Account> GetByEmployeeId(EmployeeId employeeId);
    Task<IReadOnlyCollection<Account>> ListAll();
}
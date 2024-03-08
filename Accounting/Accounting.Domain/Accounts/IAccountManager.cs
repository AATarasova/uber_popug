namespace Accounting.Domain.Accounts;

public interface IAccountManager
{
    Task CreateAccount(EmployeeId employeeId);
    Task DeleteAccount(EmployeeId employeeId);
    Task<bool> CheckExists(EmployeeId employeeId);
    Task<IReadOnlyCollection<Account>> ListAll();
}
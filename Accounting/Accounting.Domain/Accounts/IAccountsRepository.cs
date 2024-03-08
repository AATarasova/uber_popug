namespace Accounting.Domain.Accounts;

internal interface IAccountsRepository
{
    Task Add(EmployeeId employeeId);
    Task Update(EmployeeId employeeId, long sum);
}
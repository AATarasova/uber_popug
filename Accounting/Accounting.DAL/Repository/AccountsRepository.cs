using Accounting.DAL.Context;
using Accounting.Domain.Accounts;
using DbAccount = Accounting.Domain.Models.Account;

namespace Accounting.DAL.Repository;

public class AccountsRepository(AccountingDbContext dbContext) : IAccountsRepository
{
    public async Task Add(EmployeeId employeeId)
    {
        var account = new DbAccount
        {
            EmployeeId = employeeId.Value
        };
        await dbContext.Accounts.AddAsync(account);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(EmployeeId employeeId, long sum)
    {
        var account = dbContext.Accounts.First(a => a.EmployeeId == employeeId.Value);
        account.Balance = sum;
        await dbContext.SaveChangesAsync();
    }
}
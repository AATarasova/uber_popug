using Accounting.DAL.Context;
using Accounting.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
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

    public async Task Delete(EmployeeId employeeId)
    {
        var account = dbContext.Accounts.First(a => a.EmployeeId == employeeId.Value);
        dbContext.Accounts.Remove(account);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(EmployeeId employeeId)
    {
        return await dbContext.Accounts.AnyAsync(a => a.EmployeeId == employeeId.Value);
    }

    public Task<Account> GetByEmployeeId(EmployeeId employeeId)
    {
        var account = dbContext.Accounts.First(a => a.EmployeeId == employeeId.Value);
        return Task.FromResult(Convert(account)); 
    }

    public Task<IReadOnlyCollection<Account>> ListAll()
    {
        var accounts = dbContext.Accounts
            .Select(Convert)
            .ToList() as IReadOnlyCollection<Account>;
        return Task.FromResult(accounts);
    }

    public async Task Update(EmployeeId employeeId, long sum)
    {
        var account = dbContext.Accounts.First(a => a.EmployeeId == employeeId.Value);
        account.Balance = sum;
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(IReadOnlyDictionary<EmployeeId, long> balanceChanges)
    {
        var accounts = dbContext.Accounts.ToDictionary(a => a.EmployeeId);
        foreach (var (employeeId, balanceChange) in balanceChanges)
        {
            accounts[employeeId.Value].Balance += balanceChange;
        }
        await dbContext.SaveChangesAsync();
    }

    private Account Convert(DbAccount dbAccount) => new()
    {
        Balance = dbAccount.Balance,
        EmployeeId = new EmployeeId(dbAccount.EmployeeId),
        Id = new AccountId(dbAccount.Id)
    };
}
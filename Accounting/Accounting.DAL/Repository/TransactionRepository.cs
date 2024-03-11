using Accounting.DAL.Context;
using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;
using Accounting.Domain.Transactions;
using DbTransaction = Accounting.Domain.Models.Transaction;
using Task = System.Threading.Tasks.Task;

namespace Accounting.DAL.Repository;

public class TransactionRepository(AccountingDbContext dbContext, IAccountsRepository accountsRepository) : ITransactionRepository
{
    public async Task Add(AddTransactionDto transactionInfo)
    {
        var account = await accountsRepository.GetByEmployeeId(transactionInfo.TargetEmployeeId);
        
        var sumChange = GetBalanceChange(transactionInfo.TransactionType, transactionInfo.Sum);
        // update account balance
        var dbAccount = dbContext.Accounts.First(a => a.EmployeeId == transactionInfo.TargetEmployeeId.Value);
        dbAccount.Balance = account.Balance + sumChange;
        // add transaction in db
        await dbContext.Transactions.AddAsync(Convert(transactionInfo, account.Id));
        // save changes as one transaction
        await dbContext.SaveChangesAsync();
    }

    public async Task Add(IReadOnlyCollection<AddTransactionDto> transactionInfos)
    {
        var accounts = (await accountsRepository.ListAll())
            .ToDictionary(a => a.EmployeeId, a => a.Id);
        
        // update accounts balance
        var dbAccounts = dbContext.Accounts.ToDictionary(a => a.EmployeeId);
        foreach (var transactionInfo in transactionInfos)
        {
            dbAccounts[transactionInfo.TargetEmployeeId.Value].Balance += GetBalanceChange(transactionInfo.TransactionType, transactionInfo.Sum);
        }
        
        // add transaction in db
        var transactions = transactionInfos.Select(t => Convert(t, accounts[t.TargetEmployeeId]));
        await dbContext.Transactions.AddRangeAsync(transactions);
       
        // save changes as one db transaction
        await dbContext.SaveChangesAsync();
    }

    public Task<IReadOnlyCollection<Transaction>> ListByDate(DateTime completedDate)
    {
        var transactions = dbContext.Transactions.Where(t =>
            t.TransactionDate.Date == completedDate.Date);
        return Task.FromResult(transactions.Select(t => new Transaction
            {
                TargetAccountId = new AccountId(t.TargetAccountId),
                Sum = t.Sum,
                TransactionDate = t.TransactionDate,
                SourceTaskId = t.TaskId.HasValue ? new TaskId(t.TaskId.Value) : null,
                TransactionType = t.TransactionType
            })
            .ToList() as IReadOnlyCollection<Transaction>);
    }

    private DbTransaction Convert(AddTransactionDto transactionDto, AccountId accountId) => new()
    {
        TargetAccountId = accountId.Value,
        Sum = transactionDto.Sum,
        TransactionType = transactionDto.TransactionType,
        TaskId = transactionDto.SourceTaskId?.Value
    };
    
    private long GetBalanceChange(TransactionType type, ulong sum) => type switch
    {
        TransactionType.CompletedTaskPayment => (long)sum,
        TransactionType.AssignedTaskWithdrawal => -1 * (long)sum,
        TransactionType.SalaryPayment => -1 * (long)sum,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}
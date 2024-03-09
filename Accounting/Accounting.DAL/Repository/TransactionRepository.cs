using Accounting.DAL.Context;
using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;
using Accounting.Domain.Transactions;
using DbTransaction = Accounting.Domain.Models.Transaction;
using Task = System.Threading.Tasks.Task;

namespace Accounting.DAL.Repository;

public class TransactionRepository(AccountingDbContext dbContext) : ITransactionRepository
{
    public async Task Add(AccountId targetAccountId, TransactionType type, ulong sum)
    {
        var transaction = new DbTransaction()
        {
            TargetAccountId = targetAccountId.Value,
            Sum = sum,
            TransactionType = type
        };
        await dbContext.Transactions.AddAsync(transaction);
    }

    public async Task<IReadOnlyCollection<Transaction>> ListByDate(DateTime completedDate)
    {
        var transactions =  await Task.FromResult(dbContext.Transactions.Where(t => t.TransactionDate == completedDate));
        return transactions.Select(t => new Transaction
            {
                TargetAccountId = new AccountId(t.TargetAccountId),
                Sum = t.Sum,
                TransactionDate = t.TransactionDate,
                SourceTaskId = new TaskId(t.TaskId),
                TransactionType = t.TransactionType
            })
            .ToList();
    }
}
using Accounting.Domain.Accounts;

namespace Accounting.Domain.Transactions;

public interface ITransactionRepository
{
    Task Add(AccountId targetAccountId, TransactionType type, ulong sum);
    Task<IReadOnlyCollection<Transaction>> ListByDate(DateTime completedDate);
}
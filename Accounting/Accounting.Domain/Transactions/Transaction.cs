using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;

namespace Accounting.Domain.Transactions;

public class Transaction
{
    public AccountId TargetAccountId { get; init; }
    public ulong Sum { get; init; }
    public DateTime TransactionDate { get; init; }
    public TaskId? SourceTaskId { get; init; }
    public TransactionType TransactionType { get; init; }
}
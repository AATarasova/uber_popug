using Accounting.Domain.Transactions;

namespace Accounting.Domain.Models;

public class Transaction
{
    public int Id { get; init; }
    public int TargetAccountId { get; init; }
    public ulong Sum { get; init; }
    public DateTime TransactionDate { get; init; }
    public Guid? TaskId { get; init; }
    
    public TransactionType TransactionType { get; init; }
}
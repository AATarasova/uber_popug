namespace Dashboard.Domain.Transactions;

public class Transaction
{
    public Guid TargetAccountId { get; init; }
    public ulong Sum { get; init; }
    public DateTime TransactionDate { get; init; }
    public Guid SourceTaskId { get; init; }
    public TransactionType TransactionType { get; init; }
}
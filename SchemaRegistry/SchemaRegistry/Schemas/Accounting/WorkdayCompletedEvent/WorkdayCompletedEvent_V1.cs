namespace SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;

public class WorkdayCompletedEvent_V1
{
    public DateTime Date { get; init; }
    public IReadOnlyCollection<AccountState> AccountStates { get; init; } = null!;
    public IReadOnlyCollection<Transaction> Transactions { get; init; } = null!;

    public class AccountState
    {
        public Guid EmployeeId { get; init; }
        public long Balance { get; init; }
    }
    public class Transaction
    {
        public Guid EmployeeId { get; init; }
        public ulong Sum { get; init; }
        public EventTransactionType TransactionType { get; init; }
    }
};
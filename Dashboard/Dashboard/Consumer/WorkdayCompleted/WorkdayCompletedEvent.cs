using Dashboard.Domain.Transactions;

namespace Dashboard.Consumer.WorkdayCompleted;

public class WorkdayCompletedEvent
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
        public TransactionType TransactionType { get; init; }
    }
};
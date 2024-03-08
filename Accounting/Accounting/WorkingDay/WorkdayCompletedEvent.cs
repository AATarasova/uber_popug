using Accounting.Domain.Transactions;

namespace Accounting.WorkingDay;

public class WorkdayCompletedEvent
{
    public IReadOnlyCollection<AccountState> AccountStates { get; init; }
    public IReadOnlyCollection<Transaction> Transactions { get; init; }

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
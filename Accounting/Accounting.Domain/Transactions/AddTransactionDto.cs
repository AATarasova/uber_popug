using Accounting.Domain.Accounts;
using Accounting.Domain.Tasks;

namespace Accounting.Domain.Transactions;

public class AddTransactionDto
{
    public EmployeeId TargetEmployeeId { get; init; }
    public ulong Sum { get; init; }
    public TaskId? SourceTaskId { get; init; }
    public TransactionType TransactionType { get; init; }
}
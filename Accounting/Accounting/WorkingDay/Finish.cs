using Accounting.Domain.Accounts;
using Accounting.Domain.Transactions;
using EventsManager.Domain.Producer;
using MediatR;

namespace Accounting.WorkingDay;

public static class Finish
{
    public record Command : IRequest;
    
    public class Handler(IEventProducer producer, IAccountsRepository repository, ITransactionRepository transactionRepository)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var accounts = await repository.ListAll();
            var accountStates = accounts
                .Select(a => new WorkdayCompletedEvent.AccountState
                {
                    Balance = a.Balance,
                    EmployeeId = a.EmployeeId.Value
                })
                .ToList();

            var today = DateTime.Today;
            var employeeByAccount = accounts.ToDictionary(a => a.Id, a => a.EmployeeId);
            var transactions = await transactionRepository.ListByDate(today);
            var sharingTransactions = transactions
                .Where(t => t.TransactionType != TransactionType.SalaryPayment)
                .Select(t => new WorkdayCompletedEvent.Transaction
                {
                    TransactionType = t.TransactionType,
                    EmployeeId = employeeByAccount[t.TargetAccountId].Value,
                    Sum = t.Sum
                })
                .ToList();
            await producer.Produce("accounts-state", today, new WorkdayCompletedEvent
            {
                Date = today,
                AccountStates = accountStates,
                Transactions = sharingTransactions
            });
        }
    }
}
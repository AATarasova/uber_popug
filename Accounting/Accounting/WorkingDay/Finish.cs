using Accounting.Domain.Accounts;
using Accounting.Domain.Transactions;
using EventsManager.Domain.Producer;
using MediatR;

namespace Accounting.WorkingDay;

public static class Finish
{
    public record Command : IRequest;

    public class Handler(IEventProducer producer, IAccountsRepository repository,
            ITransactionRepository transactionRepository, ProducedEventsFactory producedEventsFactory)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var today = DateTime.Now;
            var accounts = await repository.ListAll();
            var transactions = await transactionRepository.ListByDate(today);

            var @event = await producedEventsFactory.CreateWorkdayCompletedEvent(accounts, transactions, today);
            await producer.Produce("workday_results", today.Date.ToShortDateString(), @event);
        }
    }
}
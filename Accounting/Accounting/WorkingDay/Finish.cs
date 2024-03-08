using EventManager.Domain.Producer;
using EventsManager.Domain.Producer;
using JetBrains.Annotations;
using MediatR;

namespace Accounting.WorkingDay;

public static class Finish
{
    public record Command : IRequest;
    
    public class Handler(IEventProducer producer)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await producer.Produce("workday_completes", new WorkdayCompletedEvent());
        }
    }
}
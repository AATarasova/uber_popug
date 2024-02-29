using MediatR;
using TaskTracker.Domain.Tasks.Management;

namespace TaskTracker.Tasks;

public static class Reassign
{
    public record Command() : IRequest;
    
    public class Handler(ITasksManager tasksManager)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await tasksManager.Reassign();
        }
    }
}
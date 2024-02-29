using MediatR;
using TaskTracker.Domain.Tasks;
using TaskTracker.Domain.Tasks.Management;

namespace TaskTracker.Tasks;

public static class Close
{
    public record Command(int TaskId) : IRequest;
    
    public class Handler(ITasksManager tasksManager)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await tasksManager.Close(new TaskId(request.TaskId));
        }
    }
}
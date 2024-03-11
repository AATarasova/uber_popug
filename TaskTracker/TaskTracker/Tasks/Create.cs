using JetBrains.Annotations;
using MediatR;
using TaskTracker.Domain.Tasks.Management;

namespace TaskTracker.Tasks;

public static class Create
{
    public record Command(Args Args) : IRequest;
    
    [PublicAPI]
    public class Args
    {
        public string Description { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string JiraId { get; set; } = null!;
    }
    
    public class Handler(ITasksManager tasksManager)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await tasksManager.Create(request.Args.JiraId, request.Args.Title, request.Args.Description);
        }
    }
}
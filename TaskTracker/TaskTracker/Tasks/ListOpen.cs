using JetBrains.Annotations;
using MediatR;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Tasks;
using TaskTracker.Domain.Tasks.Management;
using TaskStatus = TaskTracker.Domain.Tasks.TaskStatus;

namespace TaskTracker.Tasks;

public static class ListOpen
{
    public record Query: IRequest<Response>;
    [PublicAPI]
    public record Response(IEnumerable<TaskDto> Tasks);
    [PublicAPI]
    public class TaskDto
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string Description { get; set; } = null!;
        public string CreatedDate { get; set; } = null!;
        public Guid Developer { get; set; }
    }
    
    public class Handler(ITasksManager tasksManager) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
        {
            var tasks = await tasksManager.ListOpen();

            var dtos = tasks.Select(t => new TaskDto()
            {
                Id = t.Id.Value,
                PublicId = t.PublicId,
                Description = t.Description,
                CreatedDate = t.CreatedDate.ToShortDateString(),
                Developer = t.DeveloperId.Value
            });
            return new Response(dtos);
        }
    }
}
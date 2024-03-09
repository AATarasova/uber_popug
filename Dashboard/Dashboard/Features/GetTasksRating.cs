using Dashboard.Domain.Company;
using JetBrains.Annotations;
using MediatR;

namespace Dashboard.Features;

public static class GetTasksRating
{
    public record Query(Args Args): IRequest<Response>;
    public record Args(DateTime StartDate, DateTime? FinishDate): IRequest<Response>;
    
    [PublicAPI]
    public record Response(IEnumerable<InfoByDay> InfoByDays);

    [PublicAPI]
    public record InfoByDay(DateTime Date, ulong MaxTaskCost);
    
    public class Handler(ITasksRatingRepository repository) : IRequestHandler<Query, Response>
    {
        public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
        {
            var result =
                await repository.ListByDates(query.Args.StartDate, query.Args.FinishDate ?? query.Args.StartDate);
            var statistic = result
                .Select(r => new InfoByDay(r.Date, r.MaxCost))
                .OrderBy(r => r.Date)
                .ToList();
            return new Response(statistic);
        }
    }
}
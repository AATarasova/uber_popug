using TemplateService.Domain.Templates;
using MediatR;
using JetBrains.Annotations;

namespace TemplateService.Features.TemplateData;

public static class GetTemplateData
{
    public record Query(int Id): IRequest<Response>;
    
    [PublicAPI]
    public record Response(Template Template);
    [PublicAPI]
    public record Template(int Id, string Title);
    
    public class Handler :  IRequestHandler<Query, Response>
    {
        private readonly ITemplateRepository _templateRepository;

        public Handler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
        {
            var template = await _templateRepository.GetById(new TemplateId(query.Id));

            return new Response(new Template(template.Id.Value, template.Title));
        }
    }
}
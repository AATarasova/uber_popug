using JetBrains.Annotations;
using TemplateService.Domain.Templates;
using MediatR;

namespace TemplateService.Features.TemplateData;

public static class ListTemplateData
{
    public record Query: IRequest<Response>;
    [PublicAPI]
    public record Response(IEnumerable<Template> Templates);
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
            var templates = await _templateRepository.ListAll();

            var dtos = templates.Select(t => new Template(t.Id.Value, t.Title));
            return new Response(dtos);
        }
    }
}
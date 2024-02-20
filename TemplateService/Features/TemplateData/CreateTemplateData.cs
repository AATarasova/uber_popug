using MediatR;
using TemplateService.Domain.Templates;

namespace TemplateService.Features.TemplateData;

public static class CreateTemplateData
{
    public record Command(int Id, string Title) : IRequest;
    
    public class Handler: IRequestHandler<Command>
    {
        private readonly ITemplateRepository _templateRepository;

        public Handler(ITemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var template = new Template()
            {
                Id = new TemplateId(request.Id),
                Title = request.Title
            };
            await _templateRepository.Create(template);
        }
    }
}
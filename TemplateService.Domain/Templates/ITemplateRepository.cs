namespace TemplateService.Domain.Templates;

public interface ITemplateRepository
{
    Task<Template> GetById(TemplateId id);
    Task Create(Template template);
}
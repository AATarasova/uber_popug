namespace TemplateService.Domain.Templates;

public interface ITemplateRepository
{
    Task<Template> GetById(TemplateId id);
    Task<IReadOnlyCollection<Template>> ListAll();
    Task Create(Template template);
}
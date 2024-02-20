using Microsoft.EntityFrameworkCore;
using TemplateService.DAL.Context;
using TemplateService.Domain.Templates;
using DbTemplate = TemplateService.Domain.Models.Template;

namespace TemplateService.DAL.Repository;

public class TemplateRepository : ITemplateRepository
{
    private readonly TemplateDbContext _dbContext;
            
    public TemplateRepository(TemplateDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Template> GetById(TemplateId id)
    {
        var dbTemplate = await _dbContext.Templates.FirstAsync(t => t.Id == id.Value);
        return Convert(dbTemplate);
    }

    public async Task<IReadOnlyCollection<Template>> ListAll()
    {
        var templates = _dbContext.Templates
            .Select(Convert)
            .ToList();
        return await Task.FromResult(templates);
    }

    public async Task Create(Template template)
    {
        var dbTemplate = new DbTemplate()
        {
            Id = template.Id.Value,
            Title = template.Title
        };
        await _dbContext.AddAsync(dbTemplate);
    }

    private Template Convert(DbTemplate t) => new()
    {
        Id = new TemplateId(t.Id),
        Title = t.Title
    };
}
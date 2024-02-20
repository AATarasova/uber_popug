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
        return new Template
        {
            Id = id,
            Title = dbTemplate.Title
        };
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
}
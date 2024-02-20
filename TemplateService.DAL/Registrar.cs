using Microsoft.Extensions.DependencyInjection;
using TemplateService.DAL.Repository;
using TemplateService.Domain.Templates;

namespace TemplateService.DAL;

public static class Registrar
{
    public static void RegisterTemplateDAL(this IServiceCollection services)
    {
        services.AddScoped<ITemplateRepository, TemplateRepository>();
    }
}
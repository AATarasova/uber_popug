using System.Reflection;
using TemplateService.DAL;
using TemplateService.DAL.Context;

namespace TemplateService;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers();
        
        serviceCollection.AddEndpointsApiExplorer();
        var assembly = typeof(Startup).GetTypeInfo().Assembly;
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        serviceCollection.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+','.'));
        });
        
        serviceCollection.AddDbContext<TemplateDbContext>();
        serviceCollection.RegisterTemplateDAL();
    }

}
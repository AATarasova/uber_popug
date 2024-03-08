using EventManager.Domain.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Domain;

public static class Registrar
{
    public static void RegisterEventsDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventProducer>(p => new EventProducer(configuration["Kafka:BootstrapServers"]));
    }
}
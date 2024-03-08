using EventManager.Domain.Producer;
using EventsManager.Domain.Consumer;
using EventsManager.Domain.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsManager.Domain;

public static class Registrar
{
    public static void RegisterEventsDomain(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["Kafka:BootstrapServers"];
        services.AddScoped<IEventProducer>(p => new EventProducer(url));
        services.AddScoped<IEventConsumer>(p => new EventConsumer(url));
    }
}
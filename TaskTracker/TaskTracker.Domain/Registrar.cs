using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Domain.Tasks.Management;

namespace TaskTracker.Domain;

public static class Registrar
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        services.AddScoped<ITasksManager, TasksManager>();
    }
}
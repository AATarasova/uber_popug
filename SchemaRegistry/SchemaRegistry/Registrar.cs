using Microsoft.Extensions.DependencyInjection;
using SchemaRegistry.Schemas.Accounting.WorkdayCompletedEvent;
using SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;
using SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;
using SchemaRegistry.Schemas.Tasks.TaskCreatedEvent;
using SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

namespace SchemaRegistry;

public static class Registrar
{
    public static IServiceCollection AddSchemaRegistry(this IServiceCollection services)
    {
        services.AddSingleton<Schemas.SchemaRegistry, WorkdayCompletedEventSchemaRegistry>();
        services.AddSingleton<EmployeeCreatedEventSchemaRegistry>();
        services.AddSingleton<EmployeeRoleChangedEventSchemaRegistry>();
        services.AddSingleton<TaskCreatedEventSchemaRegistry>();
        services.AddSingleton<TaskStatusChangedEventSchemaRegistry>();
        return services;
    }  
}
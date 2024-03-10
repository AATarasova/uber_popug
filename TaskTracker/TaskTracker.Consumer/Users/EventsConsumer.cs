using System.Text.Json;
using System.Text.Json.Serialization;
using EventsManager.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using SchemaRegistry.Schemas.Employees.EmployeeCreatedEvent;
using SchemaRegistry.Schemas.Employees.EmployeeRoleChangedEvent;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Consumer.Users;

public class EventsConsumer(IServiceScopeFactory serviceScopeFactory,
    EmployeeCreatedEventSchemaRegistry employeeCreatedEventSchemaRegistry,
    EmployeeRoleChangedEventSchemaRegistry employeeRoleChangedEventSchemaRegistry,
    ILogger<EventsConsumer> logger) : BackgroundService
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters ={
            new JsonStringEnumConverter()
        }
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        new Thread(async () => await StartEmployeesStreamingConsumer(stoppingToken)).Start();
        new Thread(async () => await StartRoleUpdatesConsumer(stoppingToken)).Start();
    }

    private async Task StartEmployeesStreamingConsumer(CancellationToken cancellationToken)
    {
        Thread.CurrentThread.IsBackground = true;
        logger.LogInformation("employees-streaming consumer running at: {time}", DateTimeOffset.Now);
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.SubscribeTopic("employees-streaming", AddEmployee, cancellationToken);
        }

        logger.LogInformation("employees-streaming consumer stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task StartRoleUpdatesConsumer(CancellationToken cancellationToken)
    {
        Thread.CurrentThread.IsBackground = true;
        logger.LogInformation("employee-role-updates consumer running at: {time}", DateTimeOffset.Now);
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.SubscribeTopic("employee-role-updates", ChangeRole, cancellationToken);
        }

        logger.LogInformation("employee-role-updates consumer stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task AddEmployee(string value)
    {
        Validate(employeeCreatedEventSchemaRegistry, value);
        var employeesChangedEvent = JsonSerializer.Deserialize<EmployeeCreatedEvent>(value, _serializerOptions)
                                    ?? throw new InvalidOperationException();

        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IEmployeesManager>();
        var createDeveloperDto = new Employee()
        {
            Id = new EmployeeId(employeesChangedEvent.EmployeeId),
            Role = employeesChangedEvent.Role
        };

        await manager.Create(createDeveloperDto);
        logger.LogInformation($"Employee {createDeveloperDto.Id} added.");
    }
    
    private async Task ChangeRole(string value)
    {
        Validate(employeeRoleChangedEventSchemaRegistry, value);
        var employeesChangedEvent = JsonSerializer.Deserialize<EmployeeRoleChangedEvent>(value, _serializerOptions)
                                    ?? throw new InvalidOperationException();
        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IEmployeesManager>();
        var id = new EmployeeId(employeesChangedEvent.EmployeeId);
        var employees = await manager.ListAll();
        var target = employees.First(e => e.Value == id.Value);
        await manager.UpdateRole(target, employeesChangedEvent.Role);
        logger.LogInformation($"Role for employee {employeesChangedEvent.EmployeeId} changed to {employeesChangedEvent.Role.ToString()}.");
    }
    
    private void Validate(SchemaRegistry.Schemas.SchemaRegistry schemaRegistry, string json)
    {
        var jsonSchema = schemaRegistry.GetSchemaByVersion("V1").Result;
        var validationErrors = JsonSchema.FromJsonAsync(jsonSchema).Result.Validate(json);

        if (validationErrors.Any())
        {
            throw new InvalidCastException($"Message value {json} not match to schema V1");
        }
    }
}
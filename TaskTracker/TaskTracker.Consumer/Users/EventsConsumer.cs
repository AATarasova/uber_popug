using EventsManager.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Consumer.Users;

public class EventsConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<EventsConsumer> logger) : BackgroundService
{
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
            await eventConsumer.SubscribeTopic<EmployeeCreatedEvent>("employees-streaming", AddEmployee, cancellationToken);
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
            await eventConsumer.SubscribeTopic<EmployeeRoleChangedEvent>("employee-role-updates", ChangeRole, cancellationToken);
        }

        logger.LogInformation("employee-role-updates consumer stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task AddEmployee(EmployeeCreatedEvent employeesChangedEvent)
    {
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
    
    private async Task ChangeRole(EmployeeRoleChangedEvent employeesChangedEvent)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IEmployeesManager>();
        var id = new EmployeeId(employeesChangedEvent.EmployeeId);
        var employees = await manager.ListAll();
        var target = employees.First(e => e.Value == id.Value);
        await manager.UpdateRole(target, employeesChangedEvent.Role);
        logger.LogInformation($"Role for employee {employeesChangedEvent.EmployeeId} changed to {employeesChangedEvent.Role.ToString()}.");
    }
}
using EventsManager.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Consumer.Users;

public class UserEventsConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<UserEventsConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        new Thread(async () => await Start(stoppingToken)).Start();
    }

    private async Task Start(CancellationToken cancellationToken)
    {
        Thread.CurrentThread.IsBackground = true;
        logger.LogInformation("UserCreatedConsumer running at: {time}", DateTimeOffset.Now);
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.SubscribeTopic<UserEvent>("employees", Handle, cancellationToken);
        }

        logger.LogInformation("UserCreatedConsumer stopped at: {time}", DateTimeOffset.Now);
    }

    private async Task Handle(UserEvent userEvent)
    {
        if (userEvent.Role != Role.Developer && userEvent.Type != UserEventType.UpdateRole)
        {
            return;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IEmployeesManager>();
        switch (userEvent.Type)
        {
            case UserEventType.Create:
                var createDeveloperDto = new Employee()
                {
                    Id = new EmployeeId(userEvent.Id),
                    Role = userEvent.Role
                };

                await manager.Create(createDeveloperDto);
                logger.LogInformation($"Developer {createDeveloperDto.Id} added.");
                return;
            case UserEventType.UpdateRole:
                await Update(manager, userEvent);
                return;
            case UserEventType.Delete:
                await Delete(manager, userEvent);
                return;
        }
    }

    private async Task Delete(IEmployeesManager manager, UserEvent userEvent)
    {
        var id = new EmployeeId(userEvent.Id);
        var employees = await manager.ListAll();
        var target = employees.First(e => e.Value == id.Value);
        await manager.Delete(target);
    }
    private async Task Update(IEmployeesManager manager, UserEvent userEvent)
    {
        var id = new EmployeeId(userEvent.Id);
        var employees = await manager.ListAll();
        var target = employees.First(e => e.Value == id.Value);
        await manager.Update(target, userEvent.Role);
    }
}
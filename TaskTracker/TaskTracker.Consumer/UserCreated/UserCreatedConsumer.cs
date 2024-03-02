using EventsManager.Domain.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskTracker.Domain.Employees;
using TaskTracker.Domain.Employees.Dto;

namespace TaskTracker.Consumer.UserCreated;

public class UserCreatedConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<UserCreatedConsumer> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("UserCreatedConsumer running at: {time}", DateTimeOffset.Now);
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var eventConsumer =
                    scope.ServiceProvider.GetRequiredService<IEventConsumer>();
                await eventConsumer.SubscribeTopic<UserCreatedEvent>("create_employees", HandleCreatedEvent);
            }
            logger.LogInformation("UserCreatedConsumer stopped at: {time}", DateTimeOffset.Now);
        }
    }

    private async Task HandleCreatedEvent(UserCreatedEvent userCreatedEvent)
    {
        if (userCreatedEvent.Role != Role.Developer)
        {
            return;
        }

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var manager = scope.ServiceProvider.GetRequiredService<IEmployeesManager>();      
            var createDeveloperDto = new Employee()
            {
                Id = new EmployeeId(userCreatedEvent.Id),
                Role = userCreatedEvent.Role
            };
        
            await manager.Create(createDeveloperDto);
        }
        

    }
}
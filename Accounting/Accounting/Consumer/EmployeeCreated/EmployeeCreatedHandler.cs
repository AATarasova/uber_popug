using Accounting.Domain.Accounts;
using Role = SchemaRegistry.Schemas.Employees.Role;

namespace Accounting.Consumer.EmployeeCreated;

public class EmployeeCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<EmployeeCreatedEvent> logger) : IEventHandler<EmployeeCreatedEvent>
{
    public async Task Handle(string value)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<EventsFactory>();

        var employeeCreatedEvent = await factory.CreateEmployeeCreatedEvent(value);

        if (employeeCreatedEvent.Role != Role.Developer)
        {
            return;
        }
        var manager = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
        await manager.Add(new EmployeeId(employeeCreatedEvent.EmployeeId));
        logger.LogInformation($"New account for developer {employeeCreatedEvent.EmployeeId} added.");
    }

    public string TopicName => "employees-streaming";
}
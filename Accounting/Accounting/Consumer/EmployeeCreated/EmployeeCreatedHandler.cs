using Accounting.Domain.Accounts;
using Confluent.Kafka;
using Role = SchemaRegistry.Schemas.Employees.Role;

namespace Accounting.Consumer.EmployeeCreated;

public class EmployeeCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<EmployeeCreatedHandler> logger) : IEventHandler
{
    public async Task Handle(Message<string, string> message)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<EventsFactory>();

        var employeeCreatedEvent = await factory.CreateEmployeeCreatedEvent(message.Value);

        if (employeeCreatedEvent.Role != Role.Developer)
        {
            return;
        }
        var manager = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
        var employeeId = new EmployeeId(employeeCreatedEvent.EmployeeId);
        var exists = await manager.CheckExists(employeeId);
        if (!exists)
        {
            await manager.Add(employeeId);
        }
        logger.LogInformation($"New account for developer {employeeCreatedEvent.EmployeeId} added.");
    }

    public string TopicName => "employees-streaming";
}
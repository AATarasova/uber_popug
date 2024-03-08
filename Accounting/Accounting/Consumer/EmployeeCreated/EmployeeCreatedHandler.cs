using Accounting.Domain.Accounts;

namespace Accounting.Consumer.EmployeeCreated;

public class EmployeeCreatedHandler(IServiceScopeFactory serviceScopeFactory, ILogger logger) : IEventHandler<EmployeeCreatedEvent>
{
    public async Task Handle(EmployeeCreatedEvent employeeCreatedEvent)
    {
        if (employeeCreatedEvent.Role != Role.Developer)
        {
            return;
        }
        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IAccountManager>();
        await manager.CreateAccount(new EmployeeId(employeeCreatedEvent.EmployeeId));
        logger.LogInformation($"New account for developer {employeeCreatedEvent.EmployeeId} added.");
    }

    public string TopicName => "employees-streaming";
}
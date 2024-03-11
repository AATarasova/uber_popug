using Accounting.Domain.Accounts;
using Role = SchemaRegistry.Schemas.Employees.Role;

namespace Accounting.Consumer.EmployeeRoleChanged;

public class EmployeeRoleChangedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<EmployeeRoleChangedHandler> logger) : IEventHandler<EmployeeRoleChangedEvent>
{
    public async Task Handle(string value)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<EventsFactory>();

        var employeeRoleChanged = await factory.CreateEmployeeRoleChanged(value);
        
        var employeeId = new EmployeeId(employeeRoleChanged.EmployeeId);
        var repository = scope.ServiceProvider.GetRequiredService<IAccountsRepository>();
        var alreadyExists = await repository.CheckExists(employeeId);
        if (employeeRoleChanged.Role == Role.Developer && !alreadyExists)
        {
            await repository.Add(employeeId);
            logger.LogInformation($"New account for developer {employeeRoleChanged.EmployeeId} added.");
        }
        if  (employeeRoleChanged.Role != Role.Developer && alreadyExists)
        {
            await repository.Delete(employeeId);
            logger.LogInformation(
                $"Account for developer {employeeRoleChanged.EmployeeId} deleted. Actual employee role is {employeeRoleChanged.Role.ToString()}");
        }
    }

    public string TopicName => "employee-role-updates";
}
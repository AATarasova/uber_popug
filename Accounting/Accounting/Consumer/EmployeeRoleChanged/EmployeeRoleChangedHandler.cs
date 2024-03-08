using Accounting.Domain.Accounts;

namespace Accounting.Consumer.EmployeeRoleChanged;

public class EmployeeRoleChangedHandler(IServiceScopeFactory serviceScopeFactory, ILogger logger) : IEventHandler<EmployeeRoleChangedEvent>
{
    public async Task Handle(EmployeeRoleChangedEvent employeeRoleChanged)
    {
        var employeeId = new EmployeeId(employeeRoleChanged.EmployeeId);
        using var scope = serviceScopeFactory.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IAccountManager>();
        var alreadyExists = await manager.CheckExists(employeeId);
        if (employeeRoleChanged.Role == Role.Developer && !alreadyExists)
        {
            await manager.CreateAccount(employeeId);
            logger.LogInformation($"New account for developer {employeeRoleChanged.EmployeeId} added.");
        }
        if  (employeeRoleChanged.Role != Role.Developer && alreadyExists)
        {
            await manager.DeleteAccount(employeeId);
            logger.LogInformation(
                $"Account for developer {employeeRoleChanged.EmployeeId} deleted. Actual employee role is {employeeRoleChanged.Role.ToString()}");
        }
    }

    public string TopicName => "employee-role-updates";
}
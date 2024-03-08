using Accounting.Domain.Accounts;
using Accounting.Domain.Payment;
using Accounting.Domain.Tasks;

namespace Accounting.Consumer.TaskAssigned;

public class TaskStatusChangedHandler(IServiceScopeFactory serviceScopeFactory, ILogger logger) : IEventHandler<TaskStatusChangedEvent>
{
    public async Task Handle(TaskStatusChangedEvent taskStatusChanged)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var taskId = new TaskId(taskStatusChanged.TaskId);
        var employeeId = new EmployeeId(taskStatusChanged.DeveloperId);

        if (taskStatusChanged.Status == TaskStatus.Reassigned)
        {
            await paymentService.GetPaidForAssignedTask(taskId, employeeId);
            logger.LogInformation(
                $"Payment for task {taskStatusChanged.TaskId} was gotten from {taskStatusChanged.DeveloperId}.");
        }

        if (taskStatusChanged.Status == TaskStatus.Closed)
        {
            await paymentService.PayForCompletedTask(taskId, employeeId);
            logger.LogInformation(
                $"Task cost for {taskStatusChanged.TaskId} was paid to {taskStatusChanged.DeveloperId}.");
        }
    }

    public string TopicName => "task-workflow";
}
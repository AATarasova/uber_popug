using Accounting.Domain.Accounts;
using Accounting.Domain.Payment;
using Accounting.Domain.Tasks;
using SchemaRegistry.Schemas.Tasks.TaskStatusChangedEvent;

namespace Accounting.Consumer.TaskAssigned;

public class TaskStatusChangedHandler(IServiceScopeFactory serviceScopeFactory, ILogger<TaskStatusChangedHandler> logger) : IEventHandler<TaskStatusChangedEvent_V1>
{
    public async Task Handle(string value)
    {   
        using var scope = serviceScopeFactory.CreateScope();
        var factory = scope.ServiceProvider.GetRequiredService<EventsFactory>();

        var taskStatusChanged = await factory.CreateTaskStatusChangedEvent(value);
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        var taskId = new TaskId(taskStatusChanged.TaskId);
        var employeeId = new EmployeeId(taskStatusChanged.DeveloperId);

        if (taskStatusChanged.Type == ChangedTaskType.Reassigned)
        {
            await paymentService.GetPaidForAssignedTask(taskId, employeeId);
            logger.LogInformation(
                $"Payment for task {taskStatusChanged.TaskId} was gotten from {taskStatusChanged.DeveloperId}.");
        }

        if (taskStatusChanged.Type == ChangedTaskType.Closed)
        {
            await paymentService.PayForCompletedTask(taskId, employeeId);
            logger.LogInformation(
                $"Task cost for {taskStatusChanged.TaskId} was paid to {taskStatusChanged.DeveloperId}.");
        }
    }

    public string TopicName => "task-workflow";
}
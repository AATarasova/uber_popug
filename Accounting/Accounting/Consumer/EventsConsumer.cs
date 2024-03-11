using Accounting.Consumer.EmployeeCreated;
using Accounting.Consumer.EmployeeRoleChanged;
using Accounting.Consumer.TaskAssigned;
using Accounting.Consumer.TaskCreated;
using EventsManager.Domain.Consumer;

namespace Accounting.Consumer;

public class EventsConsumer(
    IServiceScopeFactory serviceScopeFactory,
    EmployeeCreatedHandler employeeCreatedHandler,
    EmployeeRoleChangedHandler employeeRoleChangedHandler,
    TaskStatusChangedHandler taskStatusChangedHandler,
    TaskCreatedHandler taskCreatedHandler,
    ILogger<EventsConsumer> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var threads = new List<Thread>
        {
            new(async () => await StartTopicConsumer(employeeCreatedHandler, stoppingToken)),
            new(async () => await StartTopicConsumer(employeeRoleChangedHandler, stoppingToken)),
            new(async () => await StartTopicConsumer(taskStatusChangedHandler, stoppingToken)),
            new(async () => await StartTopicConsumer(taskCreatedHandler, stoppingToken)),
        };
        
        foreach (var thread in threads)
        {
            thread.Start();
        }

        return Task.CompletedTask;
    }

    private async Task StartTopicConsumer<T>(IEventHandler<T> handler, CancellationToken cancellationToken)
    {
        Thread.CurrentThread.IsBackground = true;
        logger.LogInformation("{topicName} consumer running at: {time}", handler.TopicName, DateTimeOffset.Now);
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            await eventConsumer.SubscribeTopic<T>(handler.TopicName, handler.Handle, cancellationToken);
        }

        logger.LogInformation("{topicName} consumer stopped at: {time}", handler.TopicName, DateTimeOffset.Now);
    }
}
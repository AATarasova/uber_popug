namespace EventsManager.Domain.Consumer;

public interface IEventConsumer
{
    Task SubscribeTopic(string topic, Func<string, Task> messageHandler, CancellationToken cancellationToken);
}
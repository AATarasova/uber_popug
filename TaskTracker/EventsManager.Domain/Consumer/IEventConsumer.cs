namespace EventsManager.Domain.Consumer;

public interface IEventConsumer
{
    Task SubscribeTopic<T>(string topic, Func<T, Task> messageHandler);
}
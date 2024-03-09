namespace EventsManager.Domain.Producer;

public interface IEventProducer
{
    Task Produce<T>(string topic, string key, T producedEvent);
}
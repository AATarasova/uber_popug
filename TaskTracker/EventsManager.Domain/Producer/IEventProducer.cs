namespace EventsManager.Domain.Producer;

public interface IEventProducer
{
    Task Produce(string topic, string key, string producedEvent);
}
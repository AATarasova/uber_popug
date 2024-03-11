namespace EventsManager.Domain.Producer;

public interface IEventProducer
{
    Task Produce(string topic, string version, string producedEvent);
}
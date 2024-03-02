namespace EventManager.Domain.Producer;

public interface IEventProducer
{
    Task Produce<T>(string topic, T producedEvent) where T : ProducedEvent;
}
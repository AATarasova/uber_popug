namespace EventManager.Domain.Producer;

public abstract class ProducedEvent
{
    public Guid Id { get; set; }
}
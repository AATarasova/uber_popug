namespace Accounting.Consumer;

public interface IEventHandler<T>
{
    Task Handle(T @event);
    string TopicName { get; }
}
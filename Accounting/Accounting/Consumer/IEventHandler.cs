namespace Accounting.Consumer;

public interface IEventHandler<T>
{
    Task Handle(string value);
    string TopicName { get; }
}
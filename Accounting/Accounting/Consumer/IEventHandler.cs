using Confluent.Kafka;

namespace Accounting.Consumer;

public interface IEventHandler
{
    Task Handle(Message<string, string> message);
    string TopicName { get; }
}
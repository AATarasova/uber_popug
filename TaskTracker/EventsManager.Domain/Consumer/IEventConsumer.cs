using Confluent.Kafka;

namespace EventsManager.Domain.Consumer;

public interface IEventConsumer
{
    Task SubscribeTopic(string topic, Func<Message<string, string>, Task> messageHandler, CancellationToken cancellationToken);
}
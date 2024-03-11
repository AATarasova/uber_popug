using Confluent.Kafka;

namespace EventsManager.Domain.Consumer;

public class EventConsumer : IEventConsumer
{
    private readonly IConsumer<string, string> _consumer;

    public EventConsumer(string url)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = url,
            GroupId = "TaskManagerConsumerGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
    }

    public async Task SubscribeTopic(string topic, Func<string, Task> messageHandler, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine($"Received message with guid {consumeResult.Message.Key} and value {consumeResult.Message.Value}");
                await messageHandler(consumeResult.Message.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Kafka message: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
        }

        _consumer.Close();
    }
}

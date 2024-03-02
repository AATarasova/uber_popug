using System.Text.Json;
using Confluent.Kafka;

namespace EventsManager.Domain.Consumer;

public class EventConsumer : IEventConsumer
{
    private readonly IConsumer<Guid, string> _consumer;

    public EventConsumer(string url)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = url,
            GroupId = "TaskManagerConsumerGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Guid, string>(consumerConfig).SetKeyDeserializer(new GuidSerializer()).Build();
    }

    public async Task SubscribeTopic<T>(string topic, Func<T, Task> messageHandler)
    {
        _consumer.Subscribe(topic);

        while (true)
        {
            try
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine($"Received message with guid {consumeResult.Message.Key} and value {consumeResult.Message.Value}");
                var parsed = JsonSerializer.Deserialize<T>(consumeResult.Message.Value) ?? throw new InvalidOperationException();
                await messageHandler(parsed);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing Kafka message: {ex.Message}");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(1));
        }

        _consumer.Close();
    }
}

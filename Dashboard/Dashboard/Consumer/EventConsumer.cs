using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace Dashboard.Consumer;

public class EventConsumer : IEventConsumer
{
    private readonly IConsumer<DateTime, string> _consumer;
    private readonly JsonSerializerOptions _serializerOptions;

    public EventConsumer(string url)
    {
        _serializerOptions = new JsonSerializerOptions{
            Converters ={
                new JsonStringEnumConverter()
            }
        };
        
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = url,
            GroupId = "DashboardConsumerGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<DateTime, string>(consumerConfig).Build();
    }

    public async Task SubscribeTopic<T>(string topic, Func<T, Task> messageHandler, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine($"Received message with guid {consumeResult.Message.Key} and value {consumeResult.Message.Value}");
                var parsed = JsonSerializer.Deserialize<T>(consumeResult.Message.Value, _serializerOptions) ?? throw new InvalidOperationException();
                await messageHandler(parsed);
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

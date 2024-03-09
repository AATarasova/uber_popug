using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace EventsManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<DateTime, string> _producer;
    
    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };

        _producer = new ProducerBuilder<DateTime, string>(producerConfig)
            .Build();    
    }
    
    public async Task Produce<T>(string topic, DateTime key, T producedEvent)
    {
        var serialized = JsonSerializer.Serialize(producedEvent);
        var message = new Message<DateTime, string> {Key = key, Value = serialized};

        await _producer.ProduceAsync(topic, message, CancellationToken.None);
            
        Console.WriteLine($"Produced event to topic {topic}: value = {serialized}");
        _producer.Flush(TimeSpan.FromSeconds(10));
        
    }
}
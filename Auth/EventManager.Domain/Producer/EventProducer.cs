using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace EventManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<Guid, string> _producer;
    
    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };

        _producer = new ProducerBuilder<Guid, string>(producerConfig)
            .SetKeySerializer(new GuidSerializer())
            .Build();    
    }
    
    public async Task Produce<T>(string topic, T producedEvent) where T : ProducedEvent
    {
        var serialized = JsonSerializer.Serialize(producedEvent);
        var message = new Message<Guid, string> { Value = serialized, Key = producedEvent.Id};

        await _producer.ProduceAsync(topic, message, CancellationToken.None);
            
        Console.WriteLine($"Produced event to topic {topic}: key = {producedEvent.Id} value = {serialized}");
        _producer.Flush(TimeSpan.FromSeconds(10));
        
    }
    
    public class GuidSerializer :IAsyncSerializer<Guid>
    {
        public async Task<byte[]> SerializeAsync(Guid data, SerializationContext context)
        {
            return await Task.FromResult(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));
        }
    }
}
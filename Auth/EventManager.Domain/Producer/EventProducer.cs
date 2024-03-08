using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;

namespace EventManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<Guid, string> _producer;
    private readonly JsonSerializerOptions _serializerOptions;
    
    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };
        _serializerOptions = new JsonSerializerOptions{
            Converters ={
                new JsonStringEnumConverter()
            }
        };

        _producer = new ProducerBuilder<Guid, string>(producerConfig)
            .SetKeySerializer(new GuidSerializer())
            .Build();    
    }
    
    public async Task Produce<T>(string topic, T producedEvent)
    {
        var serialized = JsonSerializer.Serialize(producedEvent, _serializerOptions);
        var message = new Message<Guid, string> { Value = serialized, Key = Guid.NewGuid()};

        await _producer.ProduceAsync(topic, message, CancellationToken.None);
            
        Console.WriteLine($"Produced event to topic {topic}: message key = {message.Key} value = {serialized}");
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
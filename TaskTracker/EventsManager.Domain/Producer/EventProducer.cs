using System.Text;
using Confluent.Kafka;

namespace EventsManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;

    public EventProducer(string configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration
        };

        var builder = new ProducerBuilder<string, string>(producerConfig);
        _producer = builder.Build();
    }

    public async Task Produce(string topic, string version, string producedEvent)
    {
        var headers = new Headers();
        headers.Add( "Version", Encoding.UTF8.GetBytes(version));
        var message = new Message<string, string>
        {
            Value = producedEvent, 
            Key = version, 
            Headers = headers
        };

        await _producer.ProduceAsync(topic, message, CancellationToken.None);

        Console.WriteLine($"Produced event to topic {topic}: key = {version} value = {producedEvent}");
    }
}
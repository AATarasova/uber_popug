using System.Text;
using Confluent.Kafka;

namespace EventsManager.Domain.Producer;

internal class EventProducer : IEventProducer
{
    private readonly IProducer<string, string> _producer;
    private List<(string, Message<string, string>)> _delayedMessages;
    private Thread retryThread;

    private async Task Retry()
    {
        Thread.CurrentThread.IsBackground = true;
        foreach (var (topic, message) in _delayedMessages)
        {
            Thread.Sleep(4000);
            await _producer.ProduceAsync(topic, message, CancellationToken.None);
        }
    }
    public EventProducer(string configuration)
    {
        retryThread = new Thread(async () => await  Retry());
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

        try
        {
            await _producer.ProduceAsync(topic, message, CancellationToken.None);
        }
        catch (ProduceException<string, string> exception)
        {
            if (exception.DeliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                _delayedMessages.Add((topic, message));
                if (!retryThread.IsAlive)
                {
                    retryThread.Start();
                }
            }
        }
        Console.WriteLine($"Produced event to topic {topic}: key = {version} value = {producedEvent}");
    }
}
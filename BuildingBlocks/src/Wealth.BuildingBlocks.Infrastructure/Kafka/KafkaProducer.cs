using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly ProducerConfig config;

    public KafkaProducer(IOptions<KafkaProducerOptions> options)
    {
        config = new ProducerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
        };
    }

    public async Task ProduceAsync<T>(string topic, IEnumerable<Message<string, T>> messages, CancellationToken token)
    {
        using var producer = new ProducerBuilder<string, T>(config)
            .Build();

        foreach (var message in messages)
            await producer.ProduceAsync(topic, message, token);
    }
}
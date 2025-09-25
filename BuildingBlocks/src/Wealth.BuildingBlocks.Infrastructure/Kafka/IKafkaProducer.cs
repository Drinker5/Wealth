using Confluent.Kafka;

namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, IEnumerable<Message<string, T>> message, CancellationToken token);
}
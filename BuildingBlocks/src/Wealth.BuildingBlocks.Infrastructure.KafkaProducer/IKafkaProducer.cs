using Confluent.Kafka;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, IEnumerable<Message<string, T>> message, CancellationToken token)
        where T : Google.Protobuf.IMessage;
}
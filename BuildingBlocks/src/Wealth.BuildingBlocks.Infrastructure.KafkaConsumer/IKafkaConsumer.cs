using Confluent.Kafka;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public interface IKafkaConsumer
{
    Task ConsumeAsync<T>(string topic, Func<ConsumeResult<string, T>, Task> handler, CancellationToken token)
        where T : Google.Protobuf.IMessage;
}
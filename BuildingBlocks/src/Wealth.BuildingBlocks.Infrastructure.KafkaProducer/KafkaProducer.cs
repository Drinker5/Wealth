using Confluent.Kafka;
using Eventso.KafkaProducer;

using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

public sealed class KafkaProducer<T>(
    IProducer producer,
    string topic,
    Func<T, long> key) : IKafkaProducer<T>
    where T : Google.Protobuf.IMessage
{
    public Task Produce(IEnumerable<T> messages, CancellationToken token)
    {
        var batch = producer.CreateBatch(topic);
        foreach (var message in messages)
            batch.Produce(key(message), message);

        return batch.Complete(token);
    }

    public Task Produce(T message, CancellationToken token)
    {
        return producer.ProduceAsync(topic, key(message), message, token);
    }
}
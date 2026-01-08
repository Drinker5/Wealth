namespace Wealth.BuildingBlocks.Application;

public interface IKafkaProducer<in T>
    where T : Google.Protobuf.IMessage
{
    Task Produce(IEnumerable<T> messages, CancellationToken token);

    Task Produce(T message, CancellationToken token);
}
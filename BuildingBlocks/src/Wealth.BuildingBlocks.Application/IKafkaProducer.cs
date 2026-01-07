namespace Wealth.BuildingBlocks.Application;

public interface IKafkaProducer
{
    Task Produce<T>(string topic, IEnumerable<BusMessage<string, T>> messages, CancellationToken token)
        where T : Google.Protobuf.IMessage;

    Task Produce<T>(string topic, BusMessage<string, T> message, CancellationToken token)
        where T : Google.Protobuf.IMessage;
}

public record struct BusMessage<T, V>
{
    public required T Key { get; init; }
    public required V Value { get; init; }
}
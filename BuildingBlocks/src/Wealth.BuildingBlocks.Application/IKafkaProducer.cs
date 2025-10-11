namespace Wealth.BuildingBlocks.Application;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, IEnumerable<BusMessage<string, T>> message, CancellationToken token)
        where T : Google.Protobuf.IMessage;
}

public record struct BusMessage<T, V>
{
    public required T Key { get; init; }
    public required V Value { get; init; }
}
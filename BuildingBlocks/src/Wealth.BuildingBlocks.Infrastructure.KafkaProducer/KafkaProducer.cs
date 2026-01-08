using Confluent.Kafka;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

public sealed class KafkaProducer(IOptions<KafkaProducerOptions> options) : IKafkaProducer
{
    private readonly ProducerConfig config = new()
    {
        BootstrapServers = options.Value.BootstrapServers,
    };

    public async Task Produce<T>(string topic, IEnumerable<BusMessage<string, T>> messages, CancellationToken token)
        where T : Google.Protobuf.IMessage
    {
        var builder = new ProducerBuilder<string, T>(config)
            .SetKeySerializer(Serializers.Utf8);

        var serializer = new ProtobufMessageSerializer<T>();
        builder = builder.SetValueSerializer(serializer);

        using var producer = builder.Build();

        foreach (var message in messages.Select(i => new Message<string, T> { Key = i.Key, Value = i.Value }))
            await producer.ProduceAsync(topic, message, token);
    }

    public async Task Produce<T>(string topic, BusMessage<string, T> message, CancellationToken token) where T : IMessage
    {
        var builder = new ProducerBuilder<string, T>(config)
            .SetKeySerializer(Serializers.Utf8);

        var serializer = new ProtobufMessageSerializer<T>();
        builder = builder.SetValueSerializer(serializer);

        using var producer = builder.Build();

        await producer.ProduceAsync(topic, new Message<string, T> { Key = message.Key, Value = message.Value }, token);
    }

    private sealed class ProtobufMessageSerializer<T> : ISerializer<T>
        where T : Google.Protobuf.IMessage
    {
        public byte[] Serialize(T data, SerializationContext context) => data.ToByteArray();
    }
}
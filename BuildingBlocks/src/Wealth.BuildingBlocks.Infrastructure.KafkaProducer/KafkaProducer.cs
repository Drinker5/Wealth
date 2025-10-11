using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

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

    public async Task ProduceAsync<T>(string topic, IEnumerable<BusMessage<string, T>> messages, CancellationToken token)
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

    private sealed class ProtobufMessageSerializer<T> : ISerializer<T>
        where T : Google.Protobuf.IMessage
    {
        public byte[] Serialize(T data, SerializationContext context)
            => Google.Protobuf.MessageExtensions.ToByteArray(data);
    }
}
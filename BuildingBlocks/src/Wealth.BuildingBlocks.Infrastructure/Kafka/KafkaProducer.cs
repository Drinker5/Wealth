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
        where T : Google.Protobuf.IMessage
    {
        var builder = new ProducerBuilder<string, T>(config)
            .SetKeySerializer(Serializers.Utf8);

        var serializer = new ProtobufMessageSerializer<T>();
        builder = builder.SetValueSerializer(serializer);

        using var producer = builder.Build();

        foreach (var message in messages)
            await producer.ProduceAsync(topic, message, token);
    }
    
    private sealed class ProtobufMessageSerializer<T> : ISerializer<T>
        where T : Google.Protobuf.IMessage
    {
        public byte[] Serialize(T data, SerializationContext context) 
            => Google.Protobuf.MessageExtensions.ToByteArray(data);
    }
}
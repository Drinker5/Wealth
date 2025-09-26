using Confluent.Kafka;
using Google.Protobuf;
using Microsoft.Extensions.Options;

namespace Wealth.BuildingBlocks.Infrastructure.Kafka;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly ConsumerConfig config;

    public KafkaConsumer(IOptions<KafkaConsumerOptions> options)
    {
        config = new ConsumerConfig
        {
            BootstrapServers = options.Value.BootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
            GroupId = options.Value.GroupId
        };
    }

    public async Task ConsumeAsync<T>(string topic, Func<ConsumeResult<string, T>, Task> handler, CancellationToken token) 
        where T : IMessage
    {
        var builder = new ConsumerBuilder<string, T>(config)
            .SetKeyDeserializer(Deserializers.Utf8);

        var deser = new ProtobufMessageDeserializer<T>();
        builder = builder.SetValueDeserializer(deser);

        using var consumer = builder.Build();
        consumer.Subscribe(topic);

        try
        {
            while (!token.IsCancellationRequested)
            {
                var result = consumer.Consume(token);
                if (result is not null)
                    await handler(result);
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    private sealed class ProtobufMessageDeserializer<T> : IDeserializer<T>
        where T : IMessage
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull || data.Length == 0)
                return default!;

            // IMessage<T> requires a Parser static property
            return (T)typeof(T)
                .GetProperty("Parser")!
                .GetValue(null)!
                .GetType()
                .GetMethod("ParseFrom", [typeof(byte[])])!
                .Invoke(
                    typeof(T).GetProperty("Parser")!.GetValue(null),
                    [data.ToArray()]
                )!;
        }
    }
}
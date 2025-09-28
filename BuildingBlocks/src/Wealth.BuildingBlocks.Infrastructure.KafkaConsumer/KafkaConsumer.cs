using Confluent.Kafka;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;

public class KafkaConsumer(IOptions<KafkaConsumerOptions> options, ILogger<KafkaConsumer> logger)
    : IKafkaConsumer
{
    private readonly ConsumerConfig config = new()
    {
        BootstrapServers = options.Value.BootstrapServers,
        AutoOffsetReset = AutoOffsetReset.Earliest,
        EnableAutoCommit = false,
        GroupId = options.Value.GroupId
    };

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

                consumer.Commit(result);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Kafka consumption was cancelled for topic {Topic}", topic);
        }
        catch (ConsumeException e)
        {
            logger.LogError(
                e,
                "Kafka consume error for topic {Topic}. Error: {ErrorReason} (Code: {ErrorCode})",
                topic, e.Error.Reason, e.Error.Code);
        }
        catch (KafkaException e)
        {
            logger.LogError(
                e,
                "Kafka exception for topic {Topic}. Error: {ErrorReason} (Code: {ErrorCode})",
                topic, e.Error.Reason, e.Error.Code);
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Unexpected error during Kafka consumption for topic {Topic}. Message: {ErrorMessage}",
                topic, e.Message);
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
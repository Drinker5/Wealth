using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.KafkaConsumer;
using Wealth.BuildingBlocks.Infrastructure.KafkaProducer;

namespace Wealth.BuildingBlocks.Infrastructure.Tests.Kafka;

[TestSubject(typeof(Infrastructure.KafkaConsumer.KafkaConsumer))]
public class KafkaTests : IClassFixture<KafkaTestFixture>
{
    private readonly KafkaTestFixture fixture;

    public KafkaTests(KafkaTestFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task WhenConsume_ProtoMessageIsDeserialized()
    {
        var topic = $"test-consumer-{Guid.NewGuid():N}";
        var groupId = $"test-group-{Guid.NewGuid():N}";

        var producerOptions = Options.Create(new KafkaProducerOptions { BootstrapServers = fixture.BootstrapServers });
        var producer = new KafkaProducer.KafkaProducer(producerOptions);
        var consumerOptions = Options.Create(new KafkaConsumerOptions { BootstrapServers = fixture.BootstrapServers, GroupId = groupId });
        var consumer = new Infrastructure.KafkaConsumer.KafkaConsumer(consumerOptions, Mock.Of<ILogger<Infrastructure.KafkaConsumer.KafkaConsumer>>());
        var expected = new MoneyProto { Amount = 99.50m, Currency = CurrencyCodeProto.Usd };

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var receivedTcs = new TaskCompletionSource<MoneyProto>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Start consumer in background
        _ = Task.Run(() => consumer.ConsumeAsync<MoneyProto>(
                topic,
                result =>
                {
                    receivedTcs.TrySetResult(result.Message.Value);
                    return Task.CompletedTask;
                },
                CancellationToken.None),
            cts.Token);

        await producer.ProduceAsync(topic, [
            new BusMessage<string, MoneyProto>
            {
                Key = Guid.NewGuid().ToString(),
                Value = expected,
            }
        ], cts.Token);

        var received = await receivedTcs.Task.WaitAsync(TimeSpan.FromSeconds(5), cts.Token);

        Assert.NotNull(received);
        Assert.Equal(expected.Currency, received.Currency);
        Assert.Equal(expected.Amount, received.Amount);
    }
}
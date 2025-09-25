using Confluent.Kafka;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Infrastructure.Kafka;
using Xunit;

namespace Wealth.BuildingBlocks.Infrastructure.Tests.Kafka;

[TestSubject(typeof(KafkaProducer))]
public class KafkaProducerTests
{
    [Fact]
    public async Task WhenProduce()
    {
        // TODO
        var producer = new KafkaProducer(Options.Create(new KafkaProducerOptions()));

        await producer.ProduceAsync("foo", [new Message<string, string>()], CancellationToken.None);
    }
}
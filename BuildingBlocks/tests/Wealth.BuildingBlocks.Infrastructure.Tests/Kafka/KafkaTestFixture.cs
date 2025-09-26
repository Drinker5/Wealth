using Testcontainers.Kafka;
using Xunit;

namespace Wealth.BuildingBlocks.Infrastructure.Tests.Kafka;

public sealed class KafkaTestFixture : IAsyncLifetime
{
    private readonly KafkaContainer kafkaContainer = new KafkaBuilder()
        .Build();

    public string BootstrapServers { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        await kafkaContainer.StartAsync();
        BootstrapServers = kafkaContainer.GetBootstrapAddress();
    }

    public async Task DisposeAsync()
    {
        await kafkaContainer.StopAsync();
    }
}




using Testcontainers.Kafka;

namespace Wealth.BuildingBlocks.Infrastructure.Tests.Kafka;

public sealed class KafkaTestFixture : IAsyncLifetime
{
    private readonly KafkaContainer kafkaContainer = new KafkaBuilder(image: "confluentinc/cp-kafka:7.5.12")
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
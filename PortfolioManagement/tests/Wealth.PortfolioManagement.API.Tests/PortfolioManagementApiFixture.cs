using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Wealth.PortfolioManagement.Application.Services;

namespace Wealth.PortfolioManagement.API.Tests;

public sealed class PortfolioManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Mock<IInstrumentService> InstrumentServiceMock { get; } = new();

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder(image: "postgres")
        .WithDatabase("PortfolioManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly KafkaContainer kafkaContainer = new KafkaBuilder(image: "confluentinc/cp-kafka:7.5.12")
        .Build();

    private string postgresConnectionString;
    private string kafkaBootstrapServers;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureHostConfiguration(config =>
        {
            var data = new Dictionary<string, string>
            {
                { "ConnectionStrings:PortfolioManagement", postgresConnectionString },
                { "KafkaProducer:BootstrapServers", kafkaBootstrapServers },
                // { "KafkaConsumer:BootstrapServers", kafkaBootstrapServers },
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services => { services.AddSingleton(InstrumentServiceMock.Object); });

        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await postgresContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await kafkaContainer.StartAsync();

        postgresConnectionString = postgresContainer.GetConnectionString();
        kafkaBootstrapServers = kafkaContainer.GetBootstrapAddress();
    }
}
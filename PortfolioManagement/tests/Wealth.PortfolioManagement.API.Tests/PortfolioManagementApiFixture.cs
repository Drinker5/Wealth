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
    private readonly IHost app;

    public Mock<IInstrumentService> InstrumentServiceMock { get; } = new();

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("PortfolioManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly KafkaContainer kafkaContainer = new KafkaBuilder()
        .Build();

    private string postgresConnectionString;
    private string kafkaBootstrapServers;

    public PortfolioManagementApiFixture()
    {
        var appBuilder = new HostApplicationBuilder();

        app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
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
        await app.StopAsync();
        app.Dispose();
        await postgresContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await kafkaContainer.StartAsync();

        await app.StartAsync();

        postgresConnectionString = postgresContainer.GetConnectionString();
        kafkaBootstrapServers = kafkaContainer.GetBootstrapAddress();
    }
}
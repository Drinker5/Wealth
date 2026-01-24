using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Wealth.InstrumentManagement.Application.Providers;
using Xunit;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class InstrumentManagementApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IHost app;

    private readonly PostgreSqlContainer postgresContainer = new PostgreSqlBuilder(image: "postgres")
        .WithDatabase("InstrumentManagement")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly KafkaContainer kafkaContainer = new KafkaBuilder(image: "confluentinc/cp-kafka:7.5.12")
        .Build();

    private string postgresConnectionString;
    private string masterConnectionString;
    private string kafkaBootstrapServers;

    public InstrumentManagementApiFixture()
    {
        var appBuilder = new HostApplicationBuilder();

        app = appBuilder.Build();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureHostConfiguration(config =>
        {
            var data = new Dictionary<string, string>
            {
                { "ConnectionStrings:InstrumentManagement", postgresConnectionString },
                { "ConnectionStrings:Master", masterConnectionString },
                { "KafkaProducer:BootstrapServers", kafkaBootstrapServers },
            };
            config.AddInMemoryCollection(data!);
        });

        builder.ConfigureServices(services => { services.AddSingleton<IInstrumentsProvider, TestTBankInstrumentsProvider>(); });

        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await app.StopAsync();
        if (app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            app.Dispose();
        }

        await postgresContainer.StopAsync();
        await kafkaContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        await app.StartAsync();
        postgresConnectionString = postgresContainer.GetConnectionString();
        masterConnectionString = new NpgsqlConnectionStringBuilder(postgresConnectionString)
        {
            Database = "postgres"
        }.ToString();
        await kafkaContainer.StartAsync();
        kafkaBootstrapServers = kafkaContainer.GetBootstrapAddress();
    }
}
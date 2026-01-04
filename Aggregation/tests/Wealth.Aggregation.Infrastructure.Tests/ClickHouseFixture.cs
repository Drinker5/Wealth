using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Octonica.ClickHouseClient;
using Testcontainers.ClickHouse;

namespace Wealth.Aggregation.Infrastructure.Tests;

public sealed class ClickHouseFixture : IAsyncLifetime
{
    private readonly ClickHouseContainer clickHouseContainer;

    public readonly IContainer migratorContainer;

    public ClickHouseFixture()
    {
        var network = new NetworkBuilder()
            .Build();

        clickHouseContainer = new ClickHouseBuilder(image: "clickhouse/clickhouse-server:25.9-alpine")
            .WithNetworkAliases("clickhouse")
            .WithNetwork(network)
            .WithDatabase("default")
            .WithUsername("default")
            .WithPassword("default")
            .Build();

        migratorContainer = new ContainerBuilder(image: "gomicro/goose:3.26.0")
            .WithNetwork(network)
            .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "Migrations"), "/migrations", AccessMode.ReadOnly)
            .DependsOn(clickHouseContainer)
            .WithEnvironment(new Dictionary<string, string>
            {
                { "GOOSE_DRIVER", "clickhouse" },
                { "GOOSE_DBSTRING", "tcp://default:default@clickhouse:9000" },
                { "GOOSE_MIGRATION_DIR", "." },
            })
            .WithCommand("sh", "-c", "goose up")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await clickHouseContainer.StartAsync();
        await migratorContainer.StartAsync();

        await WaitForDatabaseReady(TimeSpan.FromSeconds(10));
    }

    public async Task DisposeAsync()
    {
        await migratorContainer.DisposeAsync();
        await clickHouseContainer.DisposeAsync();
    }

    public string ClickHouseConnectionString
    {
        get
        {
            var properties = new Dictionary<string, string>
            {
                { "Host", clickHouseContainer.Hostname },
                { "Port", clickHouseContainer.GetMappedPublicPort(ClickHouseBuilder.NativePort).ToString() },
                { "Database", "default" },
                { "User", "default" },
                { "Password", "default" }
            };
            return string.Join(";", properties.Select(property => string.Join("=", property.Key, property.Value)));
        }
    }

    private async Task WaitForDatabaseReady(TimeSpan timeout)
    {
        var cancellationTokenSource = new CancellationTokenSource(timeout);

        while (!cancellationTokenSource.Token.IsCancellationRequested)
        {
            await using var connection = new ClickHouseConnection(ClickHouseConnectionString);
            await connection.OpenAsync(cancellationTokenSource.Token);

            await using var command = connection.CreateCommand();
            command.CommandText = """
                                  select count(*) 
                                  from system.tables
                                  where database = 'default'
                                  """;
            var result = await command.ExecuteScalarAsync(cancellationTokenSource.Token);

            if (result != null && Convert.ToInt64(result) > 0)
                return;

            await Task.Delay(1000, cancellationTokenSource.Token);
        }

        throw new TimeoutException("Database did not become ready within the specified timeout");
    }
}
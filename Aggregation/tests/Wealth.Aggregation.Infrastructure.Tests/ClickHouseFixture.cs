using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
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
        
        clickHouseContainer = new ClickHouseBuilder()
            .WithNetworkAliases("clickhouse")
            .WithNetwork(network)
            .WithDatabase("default")
            .WithUsername("default")
            .WithPassword("default")
            .Build();
        
        migratorContainer = new ContainerBuilder()
            .WithNetwork(network)
            .WithImage("gomicro/goose:3.26.0")
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
}
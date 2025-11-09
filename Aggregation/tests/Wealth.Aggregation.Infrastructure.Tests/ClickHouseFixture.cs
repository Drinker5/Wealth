using Testcontainers.ClickHouse;

namespace Wealth.Aggregation.Infrastructure.Tests;

public sealed class ClickHouseFixture : IAsyncLifetime
{
    private readonly ClickHouseContainer clickHouseContainer = new ClickHouseBuilder()
        .WithDatabase("default")
        .WithUsername("default")
        .WithPassword("default")
        .Build();

    public async Task InitializeAsync()
    {
        await clickHouseContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
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
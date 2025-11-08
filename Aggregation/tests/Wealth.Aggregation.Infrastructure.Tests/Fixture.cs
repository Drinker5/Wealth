using Testcontainers.ClickHouse;

namespace Wealth.Aggregation.Infrastructure.Tests;

public sealed class Fixture : IAsyncLifetime
{
    private readonly ClickHouseContainer clickHouseContainer = new ClickHouseBuilder()
        .Build();

    public Fixture()
    {
    }

    public async Task InitializeAsync()
    {
        await clickHouseContainer.StartAsync();
    }


    public async Task DisposeAsync()
    {
        await clickHouseContainer.StopAsync();
    }
}
using System.Data.Common;
using AutoFixture;
using JetBrains.Annotations;
using NSubstitute;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StockTradeRepository))]
public class StockTradeRepositoryTests : IClassFixture<ClickHouseFixture>
{
    private readonly ITestOutputHelper output;
    private readonly ClickHouseFixture clickHouseFixture;
    private readonly StockTradeRepository repository;
    private readonly Fixture fixture;

    public StockTradeRepositoryTests(ITestOutputHelper output, ClickHouseFixture clickHouseFixture)
    {
        fixture = new Fixture();
        this.output = output;
        this.clickHouseFixture = clickHouseFixture;

        var clickHouseConnectionStringBuilder = new ClickHouseConnectionStringBuilder(clickHouseFixture.ClickHouseConnectionString);
        var clickHouseConnectionSettings = clickHouseConnectionStringBuilder.BuildSettings();
        var connectionFactory = new ClickHouseConnectionFactory(clickHouseConnectionSettings);
        repository = new StockTradeRepository(new TableWriterBuilder(connectionFactory));
    }

    [Fact]
    public async Task WhenUpsert()
    {
        var logs = await clickHouseFixture.migratorContainer.GetLogsAsync();
        output.WriteLine(logs.Stdout);
        output.WriteLine(logs.Stderr);
        
        var op = fixture.Create<StockTrade>();

        await repository.Upsert(op, CancellationToken.None);

        await CheckOp(op, CancellationToken.None);
    }

    private async Task CheckOp(StockTrade op, CancellationToken token)
    {
        await using var connection = new ClickHouseConnection(clickHouseFixture.ClickHouseConnectionString);
        await connection.OpenAsync(token);

        var command = connection.CreateCommand("select * from stock_trade");
        var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
        {
            var id = reader.GetString(0);
            
            Assert.Equal(id, op.Id);
        }
    } 
}
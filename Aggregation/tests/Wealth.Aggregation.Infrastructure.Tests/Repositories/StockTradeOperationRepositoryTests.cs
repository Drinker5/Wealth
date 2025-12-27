using AutoFixture;
using JetBrains.Annotations;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StockTradeOperationRepository))]
public class StockTradeOperationRepositoryTests : IClassFixture<ClickHouseFixture>
{
    private readonly ITestOutputHelper output;
    private readonly ClickHouseFixture clickHouseFixture;
    private readonly StockTradeOperationRepository operationRepository;
    private readonly Fixture fixture;

    public StockTradeOperationRepositoryTests(ITestOutputHelper output, ClickHouseFixture clickHouseFixture)
    {
        fixture = new Fixture();
        this.output = output;
        this.clickHouseFixture = clickHouseFixture;

        var clickHouseConnectionStringBuilder = new ClickHouseConnectionStringBuilder(clickHouseFixture.ClickHouseConnectionString);
        var clickHouseConnectionSettings = clickHouseConnectionStringBuilder.BuildSettings();
        var connectionFactory = new ClickHouseConnectionFactory(clickHouseConnectionSettings);
        operationRepository = new StockTradeOperationRepository(new TableWriterBuilder(connectionFactory));
    }

    [Fact]
    public async Task WhenUpsert()
    {
        var logs = await clickHouseFixture.migratorContainer.GetLogsAsync();
        output.WriteLine(logs.Stdout);
        output.WriteLine(logs.Stderr);
        
        var op = fixture.Create<StockTradeOperation>();

        await operationRepository.Upsert(op, CancellationToken.None);

        await CheckOp(op, CancellationToken.None);
    }

    private async Task CheckOp(StockTradeOperation op, CancellationToken token)
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
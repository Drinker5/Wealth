using JetBrains.Annotations;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Infrastructure.Repositories;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StockAggregationRepository))]
public class StockAggregationRepositoryTests : IClassFixture<ClickHouseFixture>
{
    private readonly StockAggregationRepository repository;
    private readonly PortfolioId portfolioId = 1;
    private readonly ClickHouseConnectionFactory connectionFactory;

    public StockAggregationRepositoryTests(ClickHouseFixture clickHouseFixture)
    {
        var clickHouseConnectionStringBuilder = new ClickHouseConnectionStringBuilder(clickHouseFixture.ClickHouseConnectionString);
        var clickHouseConnectionSettings = clickHouseConnectionStringBuilder.BuildSettings();
        connectionFactory = new ClickHouseConnectionFactory(clickHouseConnectionSettings);
        repository = new StockAggregationRepository(connectionFactory);
    }

    [Fact]
    public async Task GetAggregation_WorksAsExpected()
    {
        await InitData();

        var result = await repository.GetAggregation(portfolioId, CancellationToken.None).ToListAsync();

        // Assert that we have exactly one result
        var item = Assert.Single(result);
        Assert.Multiple(() =>
        {
            Assert.Equal(1, item.StockId);
            Assert.Equal(CurrencyCode.Rub, item.Currency);
            Assert.Equal(100L, item.Quantity);
            Assert.Equal(1000m, item.TradeAmount);
            Assert.Equal(200m, item.MoneyAmount);
            Assert.Equal(42.42m, item.Price);
        });
    }

    private async Task InitData(CancellationToken token = default)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(token);

        const string insertTradeQuery =
            """
            INSERT INTO stock_trade (op_id, date, portfolio_id, stock_id, quantity, amount, currency, type)
            VALUES (@opId, @date, @portfolioId, @stockId, @quantity, @amount, @currency, @type)
            """;

        await using var tradeCommand = connection.CreateCommand(insertTradeQuery);
        tradeCommand.Parameters.AddWithValue("@opId", "trade_1");
        tradeCommand.Parameters.AddWithValue("@date", DateTime.UtcNow);
        tradeCommand.Parameters.AddWithValue("@portfolioId", portfolioId.Value);
        tradeCommand.Parameters.AddWithValue("@stockId", 1);
        tradeCommand.Parameters.AddWithValue("@quantity", 100L);
        tradeCommand.Parameters.AddWithValue("@amount", 1000m);
        tradeCommand.Parameters.AddWithValue("@currency", (byte)CurrencyCode.Rub);
        tradeCommand.Parameters.AddWithValue("@type", 1);

        await tradeCommand.ExecuteNonQueryAsync(token);

        const string insertMoneyQuery =
            """
            INSERT INTO stock_money_operation (op_id, date, portfolio_id, stock_id, amount, currency, type)
            VALUES (@opId, @date, @portfolioId, @stockId, @amount, @currency, @type)
            """;

        await using var moneyCommand = connection.CreateCommand(insertMoneyQuery);
        moneyCommand.Parameters.AddWithValue("@opId", "money_1");
        moneyCommand.Parameters.AddWithValue("@date", DateTime.UtcNow);
        moneyCommand.Parameters.AddWithValue("@portfolioId", portfolioId.Value);
        moneyCommand.Parameters.AddWithValue("@stockId", 1);
        moneyCommand.Parameters.AddWithValue("@amount", 200m);
        moneyCommand.Parameters.AddWithValue("@currency", (byte)CurrencyCode.Rub);
        moneyCommand.Parameters.AddWithValue("@type", 1);

        await moneyCommand.ExecuteNonQueryAsync(token);

        const string insertPriceQuery =
            """
            INSERT INTO instrument_price (instrument_id, instrument_type, price) 
            VALUES (@instrumentId, @instrument_type, @price)
            """;

        await using var priceCommand = connection.CreateCommand(insertPriceQuery);
        priceCommand.Parameters.AddWithValue("@instrumentId", 1);
        priceCommand.Parameters.AddWithValue("@instrument_type", (byte)1);
        priceCommand.Parameters.AddWithValue("@price", 42.42m);

        await priceCommand.ExecuteNonQueryAsync(token);

        await connection.CreateCommand("SYSTEM RELOAD DICTIONARIES").ExecuteNonQueryAsync(token);
    }
}
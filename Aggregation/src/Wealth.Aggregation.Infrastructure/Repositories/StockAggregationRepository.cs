using System.Runtime.CompilerServices;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public sealed class StockAggregationRepository(IClickHouseConnectionFactory connectionFactory) : IStockAggregationRepository
{
    public async IAsyncEnumerable<StockAggregationRaw> GetAggregation(
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(token);

        const string query =
            """
            WITH trades AS
                     (SELECT stock_id,
                             sum(quantity) as quantity,
                             sum(amount)   as trade_amount,
                             any(currency) as currency
                      FROM stock_trade FINAL
                      WHERE portfolio_id = @portfolioId
                      GROUP BY stock_id),
                 money AS
                     (SELECT stock_id,
                             sum(amount) as money_amount
                      FROM stock_money_operation FINAL
                      WHERE portfolio_id = @portfolioId
                      GROUP BY stock_id)
            SELECT t.stock_id,
                   t.currency,
                   t.quantity,
                   t.trade_amount,
                   m.money_amount,
                   dictGet('instrument_price_dictionary', 'price', (t.stock_id, 1)) as price
            FROM trades t
            LEFT JOIN money m on t.stock_id = m.stock_id;
            """;

        await using var command = connection.CreateCommand(query);
        command.Parameters.AddWithValue("@portfolioId", portfolioId.Value);

        await using var reader = await command.ExecuteReaderAsync(token);

        while (await reader.ReadAsync(token))
            yield return ReadStockAggregationRaw(reader);
    }

    private static StockAggregationRaw ReadStockAggregationRaw(ClickHouseDataReader reader)
        => new StockAggregationRaw(
            reader.GetInt32(0),
            (CurrencyCode)reader.GetByte(1),
            reader.GetInt64(2),
            reader.GetDecimal(3),
            reader.GetDecimal(4),
            reader.GetDecimal(5));
}
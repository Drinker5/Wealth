using System.Data;
using Dapper;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class PricesRepository(IClock clock, IConnectionFactory connectionFactory) : IPricesRepository
{
    private readonly IDbConnection connection = connectionFactory.CreateConnection();

    public async Task<IReadOnlyCollection<InstrumentUId>> GetOld(TimeSpan thatOld, CancellationToken token)
    {
        var olderThan = clock.Now.Subtract(thatOld);

        var command = new CommandDefinition(
            // language=postgresql
            """
            SELECT instrument_id FROM "Stocks"
            WHERE price_updated_at < @olderThan
            UNION ALL
            SELECT instrument_id FROM "Bonds"
            WHERE price_updated_at < @olderThan
            UNION ALL
            SELECT instrument_id FROM currencies
            WHERE price_updated_at < @olderThan
            """,
            parameters: new
            {
                olderThan
            },
            cancellationToken: token);

        using var reader = await connection.ExecuteReaderAsync(command);

        var result = new List<InstrumentUId>();

        while (reader.Read())
            result.Add(reader.GetGuid(0));

        return result;
    }

    public async Task<IReadOnlyCollection<InstrumentUIdPrice>> GetPrices(IReadOnlyCollection<InstrumentUId> instrumentUIds, CancellationToken token)
    {
        var command = new CommandDefinition(
            // language=postgresql
            """
            SELECT instrument_id, "Price_Amount" FROM "Stocks"
            WHERE instrument_id = ANY(@instrumentIds)
            UNION ALL
            SELECT instrument_id, "Price_Amount" FROM "Bonds"
            WHERE instrument_id = ANY(@instrumentIds)
            UNION ALL
            SELECT instrument_id, price_amount FROM currencies
            WHERE instrument_id = ANY(@instrumentIds)
            """,
            parameters: new
            {
                instrumentIds = instrumentUIds.Select(i => i.Value).ToArray()
            },
            cancellationToken: token);

        using var reader = await connection.ExecuteReaderAsync(command);

        var result = new List<InstrumentUIdPrice>();

        while (reader.Read())
            result.Add(new InstrumentUIdPrice(
                reader.GetGuid(0),
                reader.GetDecimal(1)));

        return result;
    }

    public async Task UpdatePrices(IReadOnlyCollection<InstrumentUIdPrice> prices, CancellationToken token)
    {
        var command = new CommandDefinition(
            // language=postgresql
            """
            WITH price_data AS (
                SELECT unnest(@ids) AS instrument_id,
                       unnest(@prices) AS price
            )
            UPDATE "Stocks"
            SET "Price_Amount" = data.price,
                price_updated_at = @now
            FROM price_data AS data
            WHERE "Stocks".instrument_id = data.instrument_id;

            WITH price_data AS (
                SELECT unnest(@ids) AS instrument_id,
                       unnest(@prices) AS price
            )
            UPDATE "Bonds"
            SET "Price_Amount" = data.price,
                price_updated_at = @now
            FROM price_data AS data
            WHERE "Bonds".instrument_id = data.instrument_id;

            WITH price_data AS (
                SELECT unnest(@ids) AS instrument_id,
                       unnest(@prices) AS price
            )
            UPDATE currencies
            SET price_amount = data.price,
                price_updated_at = @now
            FROM price_data AS data
            WHERE currencies.instrument_id = data.instrument_id;
            """,
            parameters: new
            {
                ids = prices.Select(i => i.InstrumentUId.Value).ToArray(),
                prices = prices.Select(i => i.Price).ToArray(),
                now = clock.Now
            },
            cancellationToken: token);

        await connection.ExecuteAsync(command);
    }
}
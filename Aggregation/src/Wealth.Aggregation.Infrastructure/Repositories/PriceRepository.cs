using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public sealed class PriceRepository(ITableWriterBuilder tableWriterBuilder, IClickHouseConnectionFactory connectionFactory) : IPriceRepository
{
    private readonly ITableWriter<InstrumentPrice> _tableWriter = tableWriterBuilder
        .For<InstrumentPrice>("instrument_price")
        .AddColumn("instrument_id", p => p.InstrumentIdType.Id.Value)
        .AddColumn("instrument_type", p => (byte)p.InstrumentIdType.Type)
        .AddColumn("price", p => p.Price.Amount)
        .Build();

    public Task ChangePrices(IReadOnlyCollection<InstrumentPrice> instrumentPrices, CancellationToken token)
    {
        return _tableWriter.Insert(instrumentPrices, token);
    }

    public async Task<IReadOnlyDictionary<InstrumentId, decimal>> GetPrices(IReadOnlyCollection<InstrumentId> instrumentIds, CancellationToken token)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(token);

        const string sql =
            // language=clickhouse
            """
            SELECT instrument_id, price 
            FROM instrument_price_view FINAL
            WHERE instrument_id IN (SELECT arrayJoin(@instrumentIds)) 
            """;

        await using var command = connection.CreateCommand(sql);
        command.Parameters.AddWithValue("@instrumentIds", instrumentIds.Select(i => i.Value).ToArray());

        var results = new Dictionary<InstrumentId, decimal>();

        await using var reader = await command.ExecuteReaderAsync(token);
        while (await reader.ReadAsync(token))
            results[reader.GetInt32(0)] = reader.GetDecimal(1);

        return results;
    }
}
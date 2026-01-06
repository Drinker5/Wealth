using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public sealed class InstrumentsRepository(WealthDbContext dbContext) : IInstrumentsRepository
{
    private readonly IDbConnection connection = dbContext.CreateConnection();

    public async Task<IReadOnlyCollection<Instrument>> GetInstruments(
        IReadOnlyCollection<InstrumentUId> instrumentIds,
        CancellationToken token)
    {
        if (instrumentIds.Count == 0)
            return [];

        const string sql =
            // language=postgresql
            """
            SELECT "Id", instrument_id, 0
            FROM "Stocks" 
            WHERE instrument_id = ANY(@instrumentIds)
            UNION ALL
            SELECT "Id", instrument_id, 1
            FROM "Bonds" 
            WHERE instrument_id = ANY(@instrumentIds)
            UNION ALL
            SELECT id, instrument_id, 2
            FROM currencies 
            WHERE instrument_id = ANY(@instrumentIds);
            """;

        var command = new CommandDefinition(
            sql,
            parameters: new
            {
                instrumentIds = instrumentIds.Select(i => i.Value).ToArray()
            },
            cancellationToken: token);

        using var reader = await connection.ExecuteReaderAsync(command);
        var result = new List<Instrument>();
        while (reader.Read())
        {
            var instrument = new Instrument(
                Id: reader.GetInt32(0),
                InstrumentUId: reader.GetGuid(1),
                Type: (InstrumentType)reader.GetByte(2));

            result.Add(instrument);
        }

        return result;
    }
}
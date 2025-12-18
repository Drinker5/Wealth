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
        IReadOnlyCollection<string> requestIsins,
        CancellationToken token)
    {
        if (requestIsins.Count == 0)
            return [];

        const string sql =
            """
            SELECT "Id", "ISIN", 0
            FROM "Stocks" 
            WHERE "ISIN" = ANY(@Isins)
            UNION ALL
            SELECT "Id", "ISIN", 1
            FROM "Bonds" 
            WHERE "ISIN" = ANY(@Isins);
            """;

        var command = new CommandDefinition(
            sql,
            parameters: new
            {
                Isins = requestIsins
            },
            cancellationToken: token);

        using var reader = await connection.ExecuteReaderAsync(command);
        var result = new List<Instrument>();
        while (reader.Read())
        {
            var instrument = new Instrument(
                Id: reader.GetInt32(0),
                Isin: reader.GetString(1),
                Type: (InstrumentType)reader.GetByte(2));

            result.Add(instrument);
        }

        return result;
    }
}
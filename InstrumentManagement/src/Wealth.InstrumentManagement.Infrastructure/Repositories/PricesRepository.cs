using System.Data;
using Dapper;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class PricesRepository(IClock clock, WealthDbContext dbContext) : IPricesRepository
{
    private readonly IDbConnection connection = dbContext.CreateConnection();

    public async Task<IReadOnlyCollection<InstrumentUId>> GetOld(TimeSpan thatOld, CancellationToken token)
    {
        var olderThan = clock.Now.Subtract(thatOld);
        using var reader = await connection.ExecuteReaderAsync(
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
            new
            {
                olderThan
            });

        var result = new List<InstrumentUId>();

        while (reader.Read())
            result.Add(reader.GetGuid(0));

        return result;
    }

    public Task UpdatePrices(IReadOnlyCollection<InstrumentUIdPrice> prices, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
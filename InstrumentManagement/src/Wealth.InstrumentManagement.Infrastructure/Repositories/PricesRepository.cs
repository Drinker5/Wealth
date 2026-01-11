using System.Data;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class PricesRepository(WealthDbContext dbContext) : IPricesRepository
{
    private readonly IDbConnection connection = dbContext.CreateConnection();
    
    public Task<IReadOnlyCollection<InstrumentUId>> GetOld(TimeSpan thatOld, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePrices(IReadOnlyCollection<InstrumentUIdPrice> prices, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
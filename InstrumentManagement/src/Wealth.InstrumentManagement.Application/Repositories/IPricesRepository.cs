using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Models;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface IPricesRepository
{
    Task<IReadOnlyCollection<InstrumentUIdType>> GetOld(TimeSpan thatOld, CancellationToken token);
    Task UpdatePrices(IReadOnlyCollection<InstrumentUIdPrice> prices, CancellationToken token);
}
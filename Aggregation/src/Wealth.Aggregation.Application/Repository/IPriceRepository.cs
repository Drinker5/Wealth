using Wealth.Aggregation.Application.Models;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Repository;

public interface IPriceRepository
{
    Task ChangePrices(IReadOnlyCollection<InstrumentPrice> instrumentPrices, CancellationToken token);

    Task<IReadOnlyDictionary<InstrumentId, decimal>> GetPrices(IReadOnlyCollection<InstrumentId> instrumentIds, CancellationToken token);
}
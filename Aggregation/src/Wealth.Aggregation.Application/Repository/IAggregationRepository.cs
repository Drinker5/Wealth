using Wealth.Aggregation.Application.Models;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Repository;

public interface IAggregationRepository
{
    IAsyncEnumerable<StockAggregationRaw> GetAggregation(
        PortfolioId portfolioId,
        IReadOnlyCollection<InstrumentId> instrumentIds,
        CancellationToken token);
}
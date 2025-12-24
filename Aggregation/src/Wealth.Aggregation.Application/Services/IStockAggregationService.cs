using Wealth.Aggregation.Application.Models;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Services;

public interface IStockAggregationService
{
    Task<IReadOnlyCollection<StockAggregation>> GetStockAggregation(
        PortfolioId portfolioId,
        StrategyId strategyId,
        CancellationToken token);
}
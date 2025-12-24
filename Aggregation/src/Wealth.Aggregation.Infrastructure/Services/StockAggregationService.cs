using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Services;

public sealed class StockAggregationService(
    IStockAggregationRepository repository,
    IInstrumentService instrumentService,
    IStrategyService strategyService) : IStockAggregationService
{
    public async Task<IReadOnlyCollection<StockAggregation>> GetStockAggregation(
        PortfolioId portfolioId,
        StrategyId strategyId,
        CancellationToken token)
    {
        var raws = await repository.GetAggregation(portfolioId, token).ToListAsync(cancellationToken: token);

        var stockIds = raws.Select(i => new StockId(i.StockId)).ToList();
        var stockInfos = await instrumentService.GetStocksInfo(stockIds, token);
        var strategy = await strategyService.GetStrategy(strategyId, token);
        var componentWeights = strategy?.Components.ToDictionary(i => i.Id, i => i.Weight) ?? [];

        return BuildStockAggregations(raws, componentWeights, stockInfos);
    }

    private static List<StockAggregation> BuildStockAggregations(
        List<StockAggregationRaw> raws,
        Dictionary<int, decimal> componentWeights,
        IReadOnlyDictionary<StockId, StockInfo> stockInfos)
    {
        var result = new List<StockAggregation>();

        foreach (var raw in raws)
        {
            var stockInfo = stockInfos[raw.StockId];
            result.Add(new StockAggregation(
                Index: stockInfo.Ticker,
                Name: stockInfo.Name,
                Weight: componentWeights.GetValueOrDefault(raw.StockId, 0),
                WeightSkipped: -1, // TODO
                UnitPrice: new Money(raw.Currency, raw.Price),
                stockInfo.DividendPerYear,
                stockInfo.LotSize,
                raw.Quantity,
                new Money(raw.Currency, raw.TradeAmount),
                new Money(raw.Currency, raw.MoneyAmount)));
        }

        return result;
    }
}
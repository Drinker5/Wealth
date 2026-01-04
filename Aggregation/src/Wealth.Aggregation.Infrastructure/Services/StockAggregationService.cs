using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Services;

public sealed class StockAggregationService(
    IAggregationRepository repository,
    IInstrumentService instrumentService,
    IStrategyService strategyService) : IStockAggregationService
{
    public async Task<IReadOnlyCollection<StockAggregation>> GetStockAggregation(
        PortfolioId portfolioId,
        StrategyId strategyId,
        CancellationToken token)
    {
        var strategy = await strategyService.GetStrategy(strategyId, token);
        if (strategy == null)
            return [];

        var componentWeights = strategy.Components.ToDictionary(i => i.IdType.Id, i => i.Weight) ?? [];
        var raws = await repository
            .GetAggregation(portfolioId, componentWeights.Keys, token)
            .ToListAsync(cancellationToken: token);

        var stockIds = raws
            .Where(i => i.InstrumentIdType.Type == InstrumentType.Stock)
            .Select(i => new StockId(i.InstrumentIdType.Id))
            .ToList();
        
        var stockInfos = await instrumentService.GetStocksInfo(stockIds, token);

        return BuildStockAggregations(raws, componentWeights, stockInfos);
    }

    private static List<StockAggregation> BuildStockAggregations(
        List<StockAggregationRaw> raws,
        Dictionary<InstrumentId, decimal> componentWeights,
        IReadOnlyDictionary<StockId, StockInfo> stockInfos)
    {
        var result = new List<StockAggregation>();

        foreach (var raw in raws)
        {
            var stockInfo = stockInfos[raw.InstrumentIdType.Id.Value];
            result.Add(new StockAggregation(
                Index: stockInfo.Ticker,
                Name: stockInfo.Name,
                Weight: componentWeights.GetValueOrDefault(raw.InstrumentIdType.Id, 0),
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
using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Application.Repository;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Services;

public sealed class StockAggregationService(
    IStockAggregationRepository repository,
    IInstrumentService instrumentService) : IStockAggregationService
{
    public async Task<IReadOnlyCollection<StockAggregation>> GetStockAggregation(
        PortfolioId portfolioId,
        CancellationToken token)
    {
        var raws = await repository.GetAggregation(portfolioId, token).ToListAsync(cancellationToken: token);

        var stockIds = raws.Select(i => new StockId(i.StockId)).ToList();
        var stockInfos = await instrumentService.GetStocksInfo(stockIds, token);
        return BuildStockAggregations(raws, stockInfos);
    }

    private IReadOnlyCollection<StockAggregation> BuildStockAggregations(
        List<StockAggregationRaw> raws,
        IReadOnlyDictionary<StockId, StockInfo> stockInfos)
    {
        var result = new List<StockAggregation>();

        foreach (var raw in raws)
        {
            var stockInfo = stockInfos[raw.StockId];
            result.Add(new StockAggregation(
                Index: "TODO",
                stockInfo.Name,
                Weight: -1, // TODO
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
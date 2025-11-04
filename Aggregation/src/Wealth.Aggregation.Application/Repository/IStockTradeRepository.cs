using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IStockTradeRepository
{
    // Task<StockAggregation?> GetStock(StockId id);
    //
    // Task<IEnumerable<StockAggregation>> GetAggregation();
    //
    // Task<StockAggregation> Create(
    //     StockId id,
    //     string name,
    //     Money stockPrice,
    //     Money dividendPerYear,
    //     int lotSize);
    //
    // Task ChangeName(StockId id, string name);
    //
    // Task ChangePrice(StockId id, Money price);
    //
    // Task ChangeDividendPerYear(StockId id, Money dividendPerYear);
    //
    // Task ChangeLotSize(StockId id, int lotSize);

    Task Upsert(StockTrade operation, CancellationToken token);

    // Task AddDividend(StockId id, Money dividend);
}
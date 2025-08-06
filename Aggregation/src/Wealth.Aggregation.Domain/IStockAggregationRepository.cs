using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Domain;

public interface IStockAggregationRepository
{
    Task<StockAggregation?> GetStock(StockId id);
    
    Task<IEnumerable<StockAggregation>> GetAggregation();
    
    Task<StockAggregation> Create(
        StockId id,
        string name,
        Money stockPrice,
        Money dividendPerYear,
        int lotSize);
    
    Task ChangeName(StockId id, string name);

    Task ChangePrice(StockId id, Money price);

    Task ChangeDividendPerYear(StockId id, Money dividendPerYear);

    Task ChangeLotSize(StockId id, int lotSize);

    Task Buy(StockId id, int quantity, Money investment);

    Task Sell(StockId id, int quantity, Money profit);

    Task AddDividend(StockId id, Money dividend);
}
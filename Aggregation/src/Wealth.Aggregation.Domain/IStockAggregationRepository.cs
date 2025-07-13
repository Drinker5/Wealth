using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Domain;

public interface IStockAggregationRepository
{
    Task<StockAggregation?> GetStock(InstrumentId id);
    
    Task<IEnumerable<StockAggregation>> GetAggregation();
    
    Task Create(
        InstrumentId id,
        string name,
        Money price,
        Money dividendPerYear,
        int lotSize);
    
    Task ChangeName(InstrumentId id, string name);

    Task ChangePrice(InstrumentId id, Money price);

    Task ChangeDividendPerYear(InstrumentId id, Money dividendPerYear);

    Task ChangeLotSize(InstrumentId id, int lotSize);

    Task Buy(InstrumentId id, int quantity, Money investment);

    Task Sell(InstrumentId id, int quantity, Money profit);

    Task AddDividend(InstrumentId id, Money dividend);
}
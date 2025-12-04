using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IStocksRepository
{
    Task<IReadOnlyCollection<Stock>> GetStocks();
    Task<Stock?> GetStock(StockId id);
    Task<Stock?> GetStock(ISIN isin);
    Task<Stock?> GetStock(FIGI figi);
    Task DeleteStock(StockId id);
    Task ChangePrice(StockId id, Money price);
    Task ChangeDividend(StockId id, Dividend dividend);

    Task<StockId> CreateStock(
        string index,
        string name,
        ISIN isin,
        FIGI figi,
        LotSize lotSize,
        CancellationToken token = default);

    Task ChangeLotSize(StockId id, int lotSize); 
    Task ChangeIndex(StockId id, string index);
}
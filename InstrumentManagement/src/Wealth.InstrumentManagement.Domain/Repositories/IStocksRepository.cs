using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IStocksRepository
{
    Task<IEnumerable<Stock>> GetStocks();
    Task<Stock?> GetStock(StockId id);
    Task<Stock?> GetStock(ISIN isin);
    Task DeleteStock(StockId id);
    Task ChangePrice(StockId id, Money price);
    Task ChangeDividend(StockId id, Dividend dividend);
    Task<StockId> CreateStock(StockId id, string name, ISIN isin);
    Task<StockId> CreateStock(string name, ISIN isin);
    Task ChangeLotSize(StockId id, int lotSize);
}
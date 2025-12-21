using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface IStocksRepository
{
    Task<IReadOnlyCollection<Stock>> GetStocks();
    Task<Stock?> GetStock(StockId id);
    Task<Stock?> GetStock(ISIN isin);
    Task<Stock?> GetStock(FIGI figi);
    Task<Stock?> GetStock(InstrumentId id);
    
    Task DeleteStock(StockId id);
    Task ChangePrice(StockId id, Money price);
    Task ChangeDividend(StockId id, Dividend dividend);

    Task<StockId> CreateStock(CreateStockCommand command, CancellationToken token = default);

    Task ChangeLotSize(StockId id, int lotSize);
    Task ChangeTicker(StockId id, Ticker ticker);
}
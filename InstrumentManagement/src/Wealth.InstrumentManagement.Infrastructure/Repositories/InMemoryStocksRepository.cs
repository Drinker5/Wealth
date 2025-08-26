using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class InMemoryStocksRepository : IStocksRepository
{
    private List<Stock> stocks = [];

    public Task<IReadOnlyCollection<Stock>> GetStocks()
    {
        return Task.FromResult<IReadOnlyCollection<Stock>>(stocks);
    }

    public Task<Stock?> GetStock(StockId id)
    {
        return Task.FromResult(stocks.FirstOrDefault(x => x.Id == id));
    }

    public Task<Stock?> GetStock(ISIN isin)
    {
        return Task.FromResult(stocks.FirstOrDefault(x => x.ISIN == isin));
    }

    public Task DeleteStock(StockId id)
    {
        stocks.RemoveAll(i => i.Id == id);
        return Task.CompletedTask;
    }

    public async Task ChangePrice(StockId id, Money price)
    {
        var stock = await GetStock(id);
        if (stock != null) stock.Price = price;
    }

    public async Task ChangeDividend(StockId id, Dividend dividend)
    {
        var stock = await GetStock(id);
        if (stock != null) stock.Dividend = dividend;
    }

    private static int currentId;

    public Task<StockId> CreateStock(string name, ISIN isin, CancellationToken token = default)
    {
        var stockId = Interlocked.Increment(ref currentId);
        var stock = Stock.Create(stockId, name, isin);
        stocks.Add(stock);
        return Task.FromResult(stock.Id);
    }

    public async Task ChangeLotSize(StockId id, int lotSize)
    {
        var stock = await GetStock(id);
        if (stock != null) stock.LotSize = lotSize;
    }
}
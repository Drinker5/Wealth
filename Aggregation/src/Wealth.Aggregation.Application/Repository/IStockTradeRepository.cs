using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IStockTradeRepository
{
    Task Upsert(StockTrade operation, CancellationToken token);
}
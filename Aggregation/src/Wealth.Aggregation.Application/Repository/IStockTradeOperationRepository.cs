using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IStockTradeOperationRepository
{
    Task Upsert(StockTradeOperation operation, CancellationToken token);
}
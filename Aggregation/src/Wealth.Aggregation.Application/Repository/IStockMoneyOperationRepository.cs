using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IStockMoneyOperationRepository
{
    Task Upsert(StockMoneyOperation operation, CancellationToken token);
}
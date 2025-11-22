using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IStockCurrencyOperationRepository
{
    Task Upsert(StockCurrencyOperation operation, CancellationToken token);
}
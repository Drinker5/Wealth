using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface ICurrencyTradeOperationRepository
{
    Task Upsert(CurrencyTradeOperation operation, CancellationToken token);
}
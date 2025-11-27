using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface ICurrencyMoneyOperationRepository
{
    Task Upsert(CurrencyMoneyOperation operation, CancellationToken token);
}
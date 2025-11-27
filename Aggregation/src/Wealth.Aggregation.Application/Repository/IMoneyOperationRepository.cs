using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IMoneyOperationRepository
{
    Task Upsert(MoneyOperation operation, CancellationToken token);
}
using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IBondCurrencyOperationRepository
{
    Task Upsert(BondCurrencyOperation operation, CancellationToken token);
}
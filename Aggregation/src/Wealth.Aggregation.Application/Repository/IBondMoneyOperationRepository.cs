using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IBondMoneyOperationRepository
{
    Task Upsert(BondMoneyOperation operation, CancellationToken token);
}
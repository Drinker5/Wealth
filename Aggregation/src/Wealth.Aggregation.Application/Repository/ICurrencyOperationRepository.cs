using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface ICurrencyOperationRepository
{
    Task Upsert(CurrencyOperation operation, CancellationToken token);
}
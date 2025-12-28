using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IOperationsRepository
{
    Task Upsert(IEnumerable<Operation> operations, CancellationToken token);
}
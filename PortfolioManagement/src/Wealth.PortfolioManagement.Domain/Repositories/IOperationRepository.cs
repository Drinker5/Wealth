using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Domain.Repositories;

public interface IOperationRepository
{
    Task UpsertOperation(Operation operation, CancellationToken token);
}
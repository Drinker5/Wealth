using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Application.Providers;

public interface IOperationProvider
{
    Task<IEnumerable<Operation>> GetOperations(DateTimeOffset from);
}
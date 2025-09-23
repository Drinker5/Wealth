using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Application.Providers;

public interface IOperationProvider
{
    IAsyncEnumerable<Operation> GetOperations(DateTimeOffset from, CancellationToken token);
}
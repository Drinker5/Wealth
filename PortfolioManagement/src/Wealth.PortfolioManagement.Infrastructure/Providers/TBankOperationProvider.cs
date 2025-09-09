using Microsoft.Extensions.Options;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public class TBankOperationProvider(IOptions<TBankOperationProviderOptions> options) : IOperationProvider
{
    public Task<IEnumerable<Operation>> GetOperations(DateTimeOffset from)
    {
        throw new NotImplementedException();
    }
}
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OperationRepository(WealthDbContext context) : IOperationRepository
{
    public async Task CreateOperation(Operation operation)
    {
        await context.InstrumentOperations.AddAsync(operation);
    }
}
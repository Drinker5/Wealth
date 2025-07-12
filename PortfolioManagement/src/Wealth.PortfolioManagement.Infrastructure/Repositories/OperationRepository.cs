using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OperationRepository(WealthDbContext context) : IOperationRepository
{
    public async Task<Guid> CreateOperation(InstrumentOperation operation)
    {
        await context.InstrumentOperations.AddAsync(operation);
        return operation.Id;
    }

    public async Task<Guid> CreateOperation(CurrencyOperation operation)
    {
        await context.CurrencyOperations.AddAsync(operation);
        return operation.Id;
    }
}
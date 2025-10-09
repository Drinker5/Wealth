using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OperationRepository(WealthDbContext dbContext) : IOperationRepository
{
    public async Task CreateOperation(Operation operation, CancellationToken token)
    {
        await dbContext.InstrumentOperations.Upsert(operation)
            .On(i => i.Id)
            .RunAsync(token);
    }
}
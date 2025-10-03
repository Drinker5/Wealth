using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OperationRepository(WealthDbContext dbContext) : IOperationRepository
{
    public async Task CreateOperation(Operation operation, CancellationToken token)
    {
        var existingOperation = await dbContext.InstrumentOperations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == operation.Id, token);

        if (existingOperation != null)
            return;

        await dbContext.InstrumentOperations.AddAsync(operation, token);
    }
}
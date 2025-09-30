using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class OperationRepository(
    IDbContextFactory<WealthDbContext> contextFactory) : IOperationRepository
{
    public async Task CreateOperation(Operation operation, CancellationToken token)
    {
        var context = await contextFactory.CreateDbContextAsync(token);
        var existingOperation = await context.InstrumentOperations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == operation.Id, token);

        if (existingOperation != null)
            return;

        await context.InstrumentOperations.AddAsync(operation, token);
    }
}
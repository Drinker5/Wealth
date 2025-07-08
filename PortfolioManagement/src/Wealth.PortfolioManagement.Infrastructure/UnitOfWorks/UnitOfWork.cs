using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

internal class UnitOfWork : IUnitOfWork
{
    private readonly WealthDbContext context;

    public UnitOfWork(WealthDbContext context)
    {
        this.context = context;
    }

    public Task<int> Commit(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}


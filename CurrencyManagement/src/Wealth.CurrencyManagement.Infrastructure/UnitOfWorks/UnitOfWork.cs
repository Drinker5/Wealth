using Microsoft.EntityFrameworkCore.Storage;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

internal class UnitOfWork : IUnitOfWork
{
    private readonly WealthDbContext context;
    private IDbContextTransaction? transaction;

    public UnitOfWork(WealthDbContext context)
    {
        this.context = context;
    }

    public async Task<IDisposable> BeginTransaction()
    {
        transaction = await context.Database.BeginTransactionAsync();
        return transaction;
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        var result = await context.SaveChangesAsync(cancellationToken);
        if (transaction != null)
            await transaction.CommitAsync(cancellationToken);

        return result;
    }
}
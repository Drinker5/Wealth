using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWork;

internal class UnitOfWork(WealthDbContext context) : IUnitOfWork
{
    public Task<int> Commit(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}


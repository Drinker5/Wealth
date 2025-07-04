using System.Data;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly WealthDbContext context;
    private readonly IDbConnection connection;
    private readonly IDbTransaction? transaction;

    public UnitOfWork(WealthDbContext context)
    {
        this.context = context;
        connection = context.CreateConnection();
        transaction = connection.BeginTransaction();
    }

    public Task<int> Commit(CancellationToken cancellationToken)
    {
        transaction?.Commit();
        return Task.FromResult(0);
    }

    public ValueTask Rollback()
    {
        transaction?.Rollback();
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        connection.Dispose();
        transaction?.Dispose();
    }
}
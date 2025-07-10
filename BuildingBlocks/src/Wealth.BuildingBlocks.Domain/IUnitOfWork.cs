namespace Wealth.BuildingBlocks.Domain;

public interface IUnitOfWork
{
    Task<IDisposable> BeginTransaction();

    Task<int> Commit(CancellationToken cancellationToken);
}
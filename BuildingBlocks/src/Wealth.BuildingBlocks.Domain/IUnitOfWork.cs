namespace Wealth.BuildingBlocks.Domain;

public interface IUnitOfWork
{
    Task<int> Commit(CancellationToken cancellationToken);
}
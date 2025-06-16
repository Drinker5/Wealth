namespace Wealth.CurrencyManagement.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> Commit(CancellationToken cancellationToken);
}
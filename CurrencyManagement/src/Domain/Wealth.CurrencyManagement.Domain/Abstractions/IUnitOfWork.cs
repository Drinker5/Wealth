namespace Wealth.CurrencyManagement.Domain.Abstractions;

public interface IUnitOfWork
{
    Task<int> Commit(CancellationToken cancellationToken);
}
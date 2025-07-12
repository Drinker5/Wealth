using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Domain.Repositories;

public interface IOperationRepository
{
    Task<Guid> CreateOperation(InstrumentOperation operation);
    Task<Guid> CreateOperation(CurrencyOperation operation);
}
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Repositories;

public interface IPortfolioRepository
{
    Task<IEnumerable<Portfolio>> GetPortfolios();
    Task<Portfolio?> GetPortfolio(PortfolioId id);
    Task RenamePortfolio(PortfolioId id, string newName);
    Task Buy(PortfolioId id, InstrumentId instrumentId, Money totalPrice, int quantity);
    Task AddCurrency(PortfolioId id, CurrencyId currencyId, decimal amount);
    Task<PortfolioId> CreatePortfolio(PortfolioId id, string requestName);
    Task<PortfolioId> CreatePortfolio(string requestName);
}
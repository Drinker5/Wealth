using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Domain.Repositories;

public interface IPortfolioRepository
{
    Task<IEnumerable<Portfolio>> GetPortfolios();
    Task<Portfolio?> GetPortfolio(PortfolioId id);
    Task RenamePortfolio(PortfolioId id, string newName);
    Task AddAsset(PortfolioId id, InstrumentId instrumentId, ISIN isin, int quantity);
    Task AddCurrency(CurrencyId currencyId, decimal amount);
    Task<PortfolioId> CreatePortfolio(string requestName);
}
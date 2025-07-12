using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.Repositories;
using Wealth.PortfolioManagement.Domain.ValueObjects;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly WealthDbContext context;

    public PortfolioRepository(WealthDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Portfolio>> GetPortfolios()
    {
        return await context.Portfolios.AsNoTracking()
            .Include(i => i.Currencies)
            .Include(i => i.Assets)
            .AsSplitQuery()
            .ToListAsync();
    }

    public Task<Portfolio?> GetPortfolio(PortfolioId id)
    {
        return context.Portfolios
            .Include(i => i.Currencies)
            .Include(i => i.Assets)
            .SingleOrDefaultAsync(i => i.Id == id);
    }

    public async Task RenamePortfolio(PortfolioId id, string newName)
    {
        var portfolio = await GetPortfolio(id);
        if (portfolio == null)
            return;

        portfolio.Rename(newName);
    }

    public async Task Buy(PortfolioId id, InstrumentId instrumentId, Money totalPrice, int quantity)
    {
        var portfolio = await GetPortfolio(id);
        if (portfolio == null)
            return;

        portfolio.Buy(instrumentId, totalPrice, quantity);
    }

    public async Task AddCurrency(PortfolioId id, CurrencyId currencyId, decimal amount)
    {
        var portfolio = await GetPortfolio(id);
        if (portfolio == null)
            return;

        portfolio.Deposit(currencyId, amount);
    }

    public Task<PortfolioId> CreatePortfolio(string requestName)
    {
        var portfolio = Portfolio.Create(requestName);
        return CreatePortfolio(portfolio);
    }

    public Task<PortfolioId> CreatePortfolio(PortfolioId id, string requestName)
    {
        var portfolio = Portfolio.Create(requestName);
        return CreatePortfolio(portfolio);
    }

    private async Task<PortfolioId> CreatePortfolio(Portfolio portfolio)
    {
        await context.Portfolios.AddAsync(portfolio);
        return portfolio.Id;
    }
}
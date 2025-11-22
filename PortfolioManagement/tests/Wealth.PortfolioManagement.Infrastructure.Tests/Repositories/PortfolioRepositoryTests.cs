using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Infrastructure.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(PortfolioRepository))]
public class PortfolioRepositoryTests
{
    private readonly PortfolioRepository repository;
    private readonly WealthDbContext context;

    public PortfolioRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        repository = new PortfolioRepository(context);
    }

    [Fact]
    public async Task WhenGetPortfolios()
    {
        var portfolios = await repository.GetPortfolios();
        
        Assert.Empty(portfolios);
    }

    [Fact]
    public async Task WhenPortfolioCreatedThenIdGenerated()
    {
        var portfolio = Portfolio.Create("Foo");

        await context.Portfolios.AddAsync(portfolio);

        Assert.NotEqual(0, portfolio.Id.Value);
    }

    [Fact]
    public async Task WhenPortfolioCreatedThenIdReturned()
    {
        var id = await CreatePortfolio("Foo");
        
        Assert.NotEqual(0, id.Value);
        var portfolio = await repository.GetPortfolio(id);
        Assert.NotNull(portfolio);
        Assert.Equal("Foo", portfolio.Name);
    }
    
    [Fact]
    public async Task WhenAddAsset()
    {
        var instrumentId = new StockId(32);
        var quantity = 32;
        var money = new Money(CurrencyCode.Rub, 23.3m);
        var id = await CreatePortfolio("Foo");

        await repository.Buy(id, instrumentId, money, quantity);

        var portfolio = await repository.GetPortfolio(id);
        Assert.NotNull(portfolio);
        var asset = Assert.Single(portfolio.Stocks);
        Assert.Equal(instrumentId, asset.StockId);
        Assert.Equal(quantity, asset.Quantity);
    }

    [Fact]
    public async Task WhenAddCurrency()
    {
        CurrencyCode currencyCode = CurrencyCode.Rub;
        var amount = 23m;
        var id = await CreatePortfolio("Foo");

        await repository.AddCurrency(id, currencyCode, amount);

        var portfolio = await repository.GetPortfolio(id);
        Assert.NotNull(portfolio);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(currencyCode, currency.Currency);
        Assert.Equal(amount, currency.Amount);
    }

    private async Task<PortfolioId> CreatePortfolio(string portfolioName)
    {
        var id = await repository.CreatePortfolio(portfolioName);
        await context.SaveChangesAsync();
        return id;
    }
}
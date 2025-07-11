using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Tests.TestHelpers;

namespace Wealth.PortfolioManagement.Domain.Tests.Portfolios;

[TestSubject(typeof(Portfolio))]
public class PortfolioTests
{
    [Fact]
    public void WhenCreate()
    {
        var portfolio = Portfolio.Create("foo");

        Assert.Equal("foo", portfolio.Name);
        Assert.Empty(portfolio.Currencies);
        Assert.Empty(portfolio.Assets);
        Assert.Equal(0, portfolio.Id.Id);
        var ev = portfolio.HasEvent<PortfolioCreated>();
        Assert.Same(portfolio, ev.Portfolio);
    }

    [Fact]
    public void WhenAddCurrency()
    {
        var portfolio = new PortfolioBuilder().Build();
        var currencyId = "FOO";
        var amount = 32;

        portfolio.AddCurrency(currencyId, amount);

        var ev = portfolio.HasEvent<CurrencyAdded>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(currencyId, ev.CurrencyId);
        Assert.Equal(amount, ev.Amount);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(currencyId, currency.CurrencyId);
        Assert.Equal(amount, currency.Amount);
    }

    [Fact]
    public void WhenAddAsset()
    {
        var portfolio = new PortfolioBuilder().Build();
        var instrumentId = new InstrumentId(Guid.NewGuid());
        var isin = new ISIN("FOO");
        var quantity = 22;

        portfolio.AddAsset(instrumentId, isin, quantity);
        
        var ev = portfolio.HasEvent<AssetAdded>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(isin, ev.ISIN);
        Assert.Equal(quantity, ev.Quantity);
        var asset = Assert.Single(portfolio.Assets);
        Assert.Equal(instrumentId, asset.InstrumentId);
        Assert.Equal(isin, asset.ISIN);
        Assert.Equal(quantity, asset.Quantity);
    }
}
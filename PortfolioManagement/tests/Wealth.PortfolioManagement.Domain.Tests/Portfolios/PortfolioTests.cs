using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.Portfolios.Events;
using Wealth.PortfolioManagement.Domain.Tests.TestHelpers;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Tests.Portfolios;

[TestSubject(typeof(Portfolio))]
public class PortfolioTests
{
    private readonly Portfolio portfolio;
    private readonly InstrumentId instrumentId = new(new Guid("00000000-0000-0000-0000-000000000001"));
    private readonly CurrencyId currencyId = "FOO";
    private readonly Money price = new Money("FOO", 123.23m);
    private readonly decimal amount = 32.32m;
    private readonly int quantity = 32;

    public PortfolioTests()
    {
        portfolio = new PortfolioBuilder().Build();
    }

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
    public void WhenCurrencyDeposit()
    {
        portfolio.Deposit(currencyId, amount);

        var ev = portfolio.HasEvent<CurrencyDeposited>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(currencyId, ev.CurrencyId);
        Assert.Equal(amount, ev.Amount);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(currencyId, currency.CurrencyId);
        Assert.Equal(amount, currency.Amount);
    }

    [Fact]
    public void WhenCurrencyWithdraw()
    {
        portfolio.Withdraw(currencyId, amount);

        var ev = portfolio.HasEvent<CurrencyWithdrew>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(currencyId, ev.CurrencyId);
        Assert.Equal(amount, ev.Amount);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(currencyId, currency.CurrencyId);
        Assert.Equal(-amount, currency.Amount);
    }

    [Fact]
    public void WhenDepositAndWithdraw()
    {
        portfolio.Deposit(currencyId, amount);
        portfolio.Withdraw(currencyId, amount);

        Assert.Empty(portfolio.Currencies);
    }

    [Fact]
    public void WhenBuy()
    {
        portfolio.Buy(instrumentId, price, quantity);

        var ev = portfolio.HasEvent<AssetBought>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.TotalPrice);
        var asset = Assert.Single(portfolio.Assets);
        Assert.Equal(instrumentId, asset.InstrumentId);
        Assert.Equal(quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.CurrencyId, currency.CurrencyId);
        Assert.Equal(-price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenSell()
    {
        portfolio.Sell(instrumentId, price, quantity);

        var ev = portfolio.HasEvent<AssetSold>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.Price);
        var asset = Assert.Single(portfolio.Assets);
        Assert.Equal(instrumentId, asset.InstrumentId);
        Assert.Equal(-quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.CurrencyId, currency.CurrencyId);
        Assert.Equal(price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenBuyAndSell()
    {
        portfolio.Buy(instrumentId, price, quantity);
        portfolio.Sell(instrumentId, price, quantity);

        Assert.Empty(portfolio.Assets);
        Assert.Empty(portfolio.Currencies);
    }

    [Fact]
    public void WhenIncome()
    {
        portfolio.Income(instrumentId, price, IncomeType.Dividend);

        var ev = portfolio.HasEvent<DividendReceived>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(price, ev.Income);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.CurrencyId, currency.CurrencyId);
        Assert.Equal(price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenExpense()
    {
        portfolio.Expense(instrumentId, price, ExpenseType.Tax);

        var ev = portfolio.HasEvent<TaxPaid>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(price, ev.Expense);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.CurrencyId, currency.CurrencyId);
        Assert.Equal(-price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenSplit()
    {
        portfolio.Buy(instrumentId, price, 100);
        var splitRatio = new SplitRatio(100, 1);

        portfolio.Split(instrumentId, splitRatio);

        var asset = Assert.Single(portfolio.Assets);
        Assert.Equal(1, asset.Quantity);
        var ev = portfolio.HasEvent<StockSplit>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(splitRatio, ev.Ratio);
    }
    
    [Fact]
    public void WhenReverseSplit()
    {
        portfolio.Buy(instrumentId, price, 100);
        var splitRatio = new SplitRatio(1, 100);

        portfolio.Split(instrumentId, splitRatio);

        var asset = Assert.Single(portfolio.Assets);
        Assert.Equal(10000, asset.Quantity);
        var ev = portfolio.HasEvent<StockSplit>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Equal(splitRatio, ev.Ratio);
    }

    [Fact]
    public void WhenDelist()
    {
        portfolio.Buy(instrumentId, price, 100);
        
        portfolio.Delist(instrumentId);
        
        var ev = portfolio.HasEvent<StockDelisted>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(instrumentId, ev.InstrumentId);
        Assert.Empty(portfolio.Assets);
    }
}
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
    private readonly StockId stockId = new(1);
    private readonly BondId bondId = new(1);
    private readonly CurrencyCode currencyCode = CurrencyCode.Rub;
    private readonly Money price = new Money(CurrencyCode.Rub, 123.23m);
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
        Assert.Empty(portfolio.Bonds);
        Assert.Empty(portfolio.Stocks);
        Assert.Equal(0, portfolio.Id.Value);
        var ev = portfolio.HasEvent<PortfolioCreated>();
        Assert.Same(portfolio, ev.Portfolio);
    }

    [Fact]
    public void WhenCurrencyDeposit()
    {
        portfolio.Deposit(new Money(this.currencyCode, amount));

        var ev = portfolio.HasEvent<CurrencyDeposited>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(this.currencyCode, ev.Money.Currency);
        Assert.Equal(amount, ev.Money.Amount);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(this.currencyCode, currency.Currency);
        Assert.Equal(amount, currency.Amount);
    }

    [Fact]
    public void WhenCurrencyWithdraw()
    {
        portfolio.Withdraw(new Money(this.currencyCode, amount));

        var ev = portfolio.HasEvent<CurrencyWithdrew>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(this.currencyCode, ev.Money.Currency);
        Assert.Equal(amount, ev.Money.Amount);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(this.currencyCode, currency.Currency);
        Assert.Equal(-amount, currency.Amount);
    }

    [Fact]
    public void WhenDepositAndWithdraw()
    {
        var money = new Money(currencyCode, amount);

        portfolio.Deposit(money);
        portfolio.Withdraw(money);

        Assert.Empty(portfolio.Currencies);
    }

    [Fact]
    public void WhenBuyStock()
    {
        portfolio.Buy(stockId, price, quantity);

        var ev = portfolio.HasEvent<StockBought>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.TotalPrice);
        var asset = Assert.Single(portfolio.Stocks);
        Assert.Equal(stockId, asset.StockId);
        Assert.Equal(quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(-price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenBuyBond()
    {
        portfolio.Buy(bondId, price, quantity);

        var ev = portfolio.HasEvent<BondBought>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(bondId, ev.BondId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.TotalPrice);
        var asset = Assert.Single(portfolio.Bonds);
        Assert.Equal(bondId, asset.BondId);
        Assert.Equal(quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(-price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenSellStock()
    {
        portfolio.Sell(stockId, price, quantity);

        var ev = portfolio.HasEvent<StockSold>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.Price);
        var asset = Assert.Single(portfolio.Stocks);
        Assert.Equal(stockId, asset.StockId);
        Assert.Equal(-quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenSellBond()
    {
        portfolio.Sell(bondId, price, quantity);

        var ev = portfolio.HasEvent<BondSold>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(bondId, ev.BondId);
        Assert.Equal(quantity, ev.Quantity);
        Assert.Equal(price, ev.Price);
        var asset = Assert.Single(portfolio.Bonds);
        Assert.Equal(bondId, asset.BondId);
        Assert.Equal(-quantity, asset.Quantity);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenBuyAndSell()
    {
        portfolio.Buy(stockId, price, quantity);
        portfolio.Buy(bondId, price, quantity);
        portfolio.Sell(stockId, price, quantity);
        portfolio.Sell(bondId, price, quantity);

        Assert.Empty(portfolio.Stocks);
        Assert.Empty(portfolio.Bonds);
        Assert.Empty(portfolio.Currencies);
    }

    [Fact]
    public void WhenIncome()
    {
        portfolio.Buy(stockId, Money.Empty, 1);

        portfolio.Dividend(stockId, price);

        var ev = portfolio.HasEvent<DividendReceived>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(price, ev.Income);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(price.Amount, currency.Amount);
    }

    [Fact]
    public void WhenStockTaxPaid()
    {
        portfolio.Buy(stockId, Money.Empty, 1);

        portfolio.Tax(stockId, price);

        var ev = portfolio.HasEvent<StockOperationTaxPaid>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(price, ev.Expense);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(-price.Amount, currency.Amount);
    }


    [Fact]
    public void WhenBondTaxPaid()
    {
        portfolio.Buy(bondId, Money.Empty, 1);

        portfolio.Tax(bondId, price);

        var ev = portfolio.HasEvent<BondOperationTaxPaid>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(bondId, ev.BondId);
        Assert.Equal(price, ev.Expense);
        var currency = Assert.Single(portfolio.Currencies);
        Assert.Equal(price.Currency, currency.Currency);
        Assert.Equal(-price.Amount, currency.Amount);
    }


    [Fact]
    public void WhenSplit()
    {
        portfolio.Buy(stockId, price, 100);
        var splitRatio = new SplitRatio(100, 1);

        portfolio.Split(stockId, splitRatio);

        var asset = Assert.Single(portfolio.Stocks);
        Assert.Equal(1, asset.Quantity);
        var ev = portfolio.HasEvent<StockSplit>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(splitRatio, ev.Ratio);
    }

    [Fact]
    public void WhenReverseSplit()
    {
        portfolio.Buy(stockId, price, 100);
        var splitRatio = new SplitRatio(1, 100);

        portfolio.Split(stockId, splitRatio);

        var asset = Assert.Single(portfolio.Stocks);
        Assert.Equal(10000, asset.Quantity);
        var ev = portfolio.HasEvent<StockSplit>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Equal(splitRatio, ev.Ratio);
    }

    [Fact]
    public void WhenDelist()
    {
        portfolio.Buy(stockId, price, 100);

        portfolio.Delist(stockId);

        var ev = portfolio.HasEvent<StockDelisted>();
        Assert.Equal(portfolio.Id, ev.PortfolioId);
        Assert.Equal(stockId, ev.StockId);
        Assert.Empty(portfolio.Stocks);
    }
}
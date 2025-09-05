using JetBrains.Annotations;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Domain.Tests;

[TestSubject(typeof(StockAggregation))]
public class StockAggregationTests
{
    private StockId id = new StockId(3);
    private Money price = new Money(CurrencyCode.RUB, 12.34m);
    private Money p1 = new Money(CurrencyCode.RUB, 1m);

    private Money dividendPerYear = new Money(CurrencyCode.RUB, 3.22m);
    private readonly StockAggregation aggregation;
    private string name = "name";
    private int lotSize = 1;

    public StockAggregationTests()
    {
        aggregation = new StockAggregation(
            id,
            name,
            price,
            dividendPerYear,
            lotSize);
    }

    [Fact]
    public void WhenCreated()
    {
        Assert.Equal(id, aggregation.Id);
        Assert.Equal(name, aggregation.Name);
        Assert.Equal(price, aggregation.StockPrice);
        Assert.Equal(0, aggregation.Quantity);
        Assert.Equal(1, aggregation.LotSize);
        Assert.Equal(dividendPerYear, aggregation.DividendPerYear);
        Assert.Equal(price.CurrencyId, aggregation.CurrentValue.CurrencyId);
        Assert.Equal(0, aggregation.CurrentValue.Amount);
        Assert.Equal(price.CurrencyId, aggregation.CurrentDividendValue.CurrencyId);
        Assert.Equal(0, aggregation.CurrentDividendValue.Amount);
        Assert.Equal(price.CurrencyId, aggregation.TotalDividends.CurrencyId);
        Assert.Equal(0, aggregation.TotalDividends.Amount);
        Assert.Equal(price.CurrencyId, aggregation.TotalInvestments.CurrencyId);
        Assert.Equal(0, aggregation.TotalInvestments.Amount);
    }

    [Fact]
    public void WhenChange()
    {
        aggregation.AddDividend(price);

        Assert.Equal(price, aggregation.TotalDividends);
    }

    [Fact]
    public void WhenBuy()
    {
        aggregation.Buy(3, p1);

        Assert.Equal(3, aggregation.Quantity);
        Assert.Equal(3 * dividendPerYear, aggregation.CurrentDividendValue);
        Assert.Equal(3 * price, aggregation.CurrentValue);
        Assert.Equal(p1, aggregation.TotalInvestments);
    }

    [Fact]
    public void WhenSell()
    {
        aggregation.Buy(3, p1 * 3);

        aggregation.Sell(1, p1);

        Assert.Equal(2, aggregation.Quantity);
        Assert.Equal(2 * dividendPerYear, aggregation.CurrentDividendValue);
        Assert.Equal(2 * price, aggregation.CurrentValue);
        Assert.Equal(2 * p1, aggregation.TotalInvestments);
    }

    [Fact]
    public void WhenChangeName()
    {
        aggregation.ChangeName("bar");

        Assert.Equal("bar", aggregation.Name);
    }


    [Fact]
    public void WhenChangePrice()
    {
        aggregation.Buy(3, price * 3);

        aggregation.ChangePrice(p1);

        Assert.Equal(p1, aggregation.StockPrice);
        Assert.Equal(3 * p1, aggregation.CurrentValue);
    }


    [Fact]
    public void WhenChangeDividendPerYear()
    {
        aggregation.Buy(3, price * 3);
        
        aggregation.ChangeDividendPerYear(p1);

        Assert.Equal(p1, aggregation.DividendPerYear);
        Assert.Equal(p1 * 3, aggregation.CurrentDividendValue);
    }

    [Fact]
    public void WhenChangeLotSize()
    {
        aggregation.ChangeLotSize(10);
        
        Assert.Equal(10, aggregation.LotSize);
    }
}
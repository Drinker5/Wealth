using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

[TestSubject(typeof(ExchangeRate))]
public class ExchangeRateTests
{
    private readonly CurrencyId c1 = "Rub";
    private readonly CurrencyId c2 = "EUR";
    private readonly CurrencyId c3 = "CNY";
    private readonly DateOnly d = new DateOnly(2020, 01, 01);
    private readonly decimal r = 4.72m;

    [Fact]
    public void WhenExchangeRateCreated()
    {
        var exchangeRate = CreateExchangeRate(c1, c2, r, d);

        Assert.Equal(c1, exchangeRate.BaseCurrencyId);
        Assert.Equal(c2, exchangeRate.TargetCurrencyId);
        Assert.Equal(r, exchangeRate.Rate);
        Assert.Equal(d, exchangeRate.ValidOnDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1e-6)]
    [InlineData(-3)]
    public void CreateExchangeRate_InvalidRate(decimal invalidRate)
    {
        Assert.ThrowsAny<Exception>(() => CreateExchangeRate(c1, c2, invalidRate, d));
    }

    [Fact]
    public void CreateExchangeRate_InvalidDate()
    {
        Assert.ThrowsAny<Exception>(() => CreateExchangeRate(c1, c2, 1m, default));
    }

    [Fact]
    public void CreateExchangeRate_InvalidCurrency()
    {
        Assert.ThrowsAny<Exception>(() => CreateExchangeRate(c1, c1, 1m, d));
    }

    [Fact]
    public void Convert_InvalidCurrency()
    {
        var exchangeRate = CreateExchangeRate(c1, c2, r, d);
        var m = new Money(c3, 100);

        Assert.ThrowsAny<Exception>(() => exchangeRate.Convert(m));
    }

    [Theory]
    [InlineData(10, 100, 1000)]
    [InlineData(0.1, 100, 10)]
    [InlineData(1, 0, 0)]
    public void WhenConvert(decimal rate, decimal amount, decimal expected)
    {
        var exchangeRate = CreateExchangeRate(c1, c2, rate, d);
        var m = new Money(c1, amount);

        var result = exchangeRate.Convert(m);
        var backResult = exchangeRate.Convert(result);

        Assert.Equal(expected, result.Amount);
        Assert.Equal(c2, result.CurrencyId);
        Assert.Equal(backResult, m);
    }

    private ExchangeRate CreateExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateOnly date)
    {
        return ExchangeRate.Create(baseCurrencyId, targetCurrencyId, rate, date);
    }
}
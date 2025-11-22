using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.Tests.TestHelpers;

public class ExchangeRateBuilder
{
    private CurrencyCode baseCurrency = CurrencyCode.Rub;
    private CurrencyCode targetCurrency = CurrencyCode.Usd;
    private decimal rate = 1.42m;
    private DateOnly date = new DateOnly(2010, 10, 10);

    public ExchangeRate Build()
    {
        return ExchangeRate.Create(baseCurrency, targetCurrency, rate, date);
    }

    public ExchangeRateBuilder SetBaseCurrency(CurrencyCode currency)
    {
        this.baseCurrency = currency;
        return this;
    }


    public ExchangeRateBuilder SetTargetCurrency(CurrencyCode currency)
    {
        this.targetCurrency = currency;
        return this;
    }

    public ExchangeRateBuilder SetRate(decimal newRate)
    {
        rate = newRate;
        return this;
    }

    public ExchangeRateBuilder SetDate(DateOnly newDate)
    {
        date = newDate;
        return this;
    }
}
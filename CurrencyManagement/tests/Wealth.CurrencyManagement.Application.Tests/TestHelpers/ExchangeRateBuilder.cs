using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.Tests.TestHelpers;

public class ExchangeRateBuilder
{
    private CurrencyId baseCurrencyId = "FOO";
    private CurrencyId targetCurrencyId = "BAR";
    private decimal rate = 1.42m;
    private DateOnly date = new DateOnly(2010, 10, 10);

    public ExchangeRate Build()
    {
        return ExchangeRate.Create(baseCurrencyId, targetCurrencyId, rate, date);
    }

    public ExchangeRateBuilder SetBaseCurrencyId(CurrencyId currencyId)
    {
        this.baseCurrencyId = currencyId;
        return this;
    }


    public ExchangeRateBuilder SetTargetCurrencyId(CurrencyId currencyId)
    {
        this.targetCurrencyId = currencyId;
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
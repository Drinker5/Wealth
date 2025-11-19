using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

#pragma warning disable CS8618

namespace Wealth.CurrencyManagement.Domain.ExchangeRates;

public class ExchangeRate : AggregateRoot
{
    private const decimal minimalRate = 1e-5m;

    public CurrencyCode BaseCurrency { get; private set; }
    public CurrencyCode TargetCurrency { get; private set; }
    public decimal Rate { get; private set; }
    public DateOnly ValidOnDate { get; private set; }

    private ExchangeRate()
    {
    }

    public static ExchangeRate Create(CurrencyCode baseCurrency, CurrencyCode targetCurrency, decimal rate, DateOnly date)
    {
        if (rate < minimalRate)
            throw new ArgumentException("Rate must be greater than 0");

        if (date == default)
            throw new ArgumentException("Date must be set");

        if (baseCurrency == targetCurrency)
            throw new ArgumentException("Currencies should be different");

        var exchangeRate = new ExchangeRate
        {
            Rate = rate,
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
            ValidOnDate = date
        };
        return exchangeRate;
    }

    public Money Convert(Money money)
    {
        if (money.Currency == BaseCurrency)
            return new Money(TargetCurrency, money.Amount * Rate);

        if (money.Currency == TargetCurrency)
            return new Money(BaseCurrency, money.Amount / Rate);

        throw new ArgumentException("Invalid currency");
    }
}
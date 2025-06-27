using Wealth.CurrencyManagement.Domain.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;

#pragma warning disable CS8618

namespace Wealth.CurrencyManagement.Domain.ExchangeRates;

public class ExchangeRate : AggregateRoot
{
    private const decimal minimalRate = 1e-5m;

    public CurrencyId BaseCurrencyId { get; private set; }
    public CurrencyId TargetCurrencyId { get; private set; }
    public decimal Rate { get; private set; }
    public DateOnly ValidOnDate { get; private set; }

    private ExchangeRate()
    {
    }

    public static ExchangeRate Create(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateOnly date)
    {
        if (rate < minimalRate)
            throw new ArgumentException("Rate must be greater than 0");

        if (date == default)
            throw new ArgumentException("Date must be set");

        if (baseCurrencyId == targetCurrencyId)
            throw new ArgumentException("Currencies should be different");

        var exchangeRate = new ExchangeRate
        {
            Rate = rate,
            BaseCurrencyId = baseCurrencyId,
            TargetCurrencyId = targetCurrencyId,
            ValidOnDate = date
        };
        return exchangeRate;
    }

    public Money Convert(Money money)
    {
        if (money.CurrencyId == BaseCurrencyId)
            return new Money(TargetCurrencyId, money.Amount * Rate);

        if (money.CurrencyId == TargetCurrencyId)
            return new Money(BaseCurrencyId, money.Amount / Rate);

        throw new ArgumentException("Invalid currency");
    }
}
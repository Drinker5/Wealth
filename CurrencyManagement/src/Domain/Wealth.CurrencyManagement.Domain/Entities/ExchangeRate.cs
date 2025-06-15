using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Entities;

public class ExchangeRate : AggregateRoot
{
    private readonly decimal minimalRate = 1e-5m;

    public ExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateTime date)
    {
        if (rate < minimalRate)
            throw new ArgumentException("Rate must be greater than 0");

        if (date == default)
            throw new ArgumentException("Date must be set");

        Rate = rate;
        BaseCurrencyId = baseCurrencyId;
        TargetCurrencyId = targetCurrencyId;
        ValidOnDate = date;
    }

    public CurrencyId BaseCurrencyId { get; }
    public CurrencyId TargetCurrencyId { get; }
    public decimal Rate { get; }
    public DateTime ValidOnDate { get; }

    public Money Convert(Money money)
    {
        if (money.CurrencyId == BaseCurrencyId)
            return new Money(TargetCurrencyId, money.Amount * Rate);

        if (money.CurrencyId == TargetCurrencyId)
            return new Money(BaseCurrencyId, money.Amount / Rate);

        throw new ArgumentException("Invalid currency");
    }
}
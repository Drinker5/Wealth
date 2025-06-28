using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record Coupon : IValueObject
{
    public Money ValuePerYear { get; init; }

    public Coupon(Money ValuePerYear)
    {
        this.ValuePerYear = ValuePerYear;
    }

    public Coupon(CurrencyId CurrencyId, decimal Amount)
        : this(new Money(CurrencyId, Amount))
    {
    }
}
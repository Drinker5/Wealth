using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record struct Coupon(Money ValuePerYear) : IValueObject
{
    public static Coupon Empty = new Coupon(Money.Empty);
    
    public Coupon(CurrencyCode Currency, decimal Amount)
        : this(new Money(Currency, Amount))
    {
    }
}
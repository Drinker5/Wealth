using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record struct Dividend(Money ValuePerYear) : IValueObject
{
    public static Dividend Empty = new Dividend(Money.Empty);
    
    public Dividend(CurrencyCode Currency, decimal Amount)
        : this(new Money(Currency, Amount))
    {
    }
}
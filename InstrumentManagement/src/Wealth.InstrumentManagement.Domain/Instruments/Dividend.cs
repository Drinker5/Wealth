using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record Dividend : IValueObject
{
    public Money ValuePerYear { get; init; }

    public Dividend(Money ValuePerYear)
    {
        this.ValuePerYear = ValuePerYear;
    }

    public Dividend(CurrencyId CurrencyId, decimal Amount)
        : this(new Money(CurrencyId, Amount))
    {
    }
}
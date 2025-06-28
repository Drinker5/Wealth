using Wealth.BuildingBlocks.Domain;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public abstract class Instrument : AggregateRoot
{
    public InstrumentId Id { get; protected set; }

    public string Name { get; protected set; }

    public ISIN ISIN { get; protected set; }

    public Money Price { get; protected set; }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new InstrumentPriceChanged
        {
            Id = Id,
            ISIN = ISIN,
            NewPrice = newPrice
        });
    }

    private void When(InstrumentPriceChanged @event)
    {
        Price = @event.NewPrice;
    }
}
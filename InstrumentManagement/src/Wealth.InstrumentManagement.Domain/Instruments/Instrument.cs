using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public abstract class Instrument : AggregateRoot
{
    public InstrumentId Id { get; protected set; }

    public string Name { get; set; }

    public ISIN ISIN { get; set; }

    public Money Price { get; set; }

    public InstrumentType Type { get; protected init; }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new InstrumentPriceChanged
        {
            InstrumentId = Id,
            ISIN = ISIN,
            NewPrice = newPrice,
            Type = Type,
        });
    }

    private void When(InstrumentPriceChanged @event)
    {
        Price = @event.NewPrice;
    }
}
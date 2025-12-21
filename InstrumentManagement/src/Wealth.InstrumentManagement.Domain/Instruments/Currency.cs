using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class Currency(CurrencyId id) : AggregateRoot
{
    public CurrencyId Id { get; private set; } = id;

    public string Name { get; set; }

    public FIGI Figi { get; set; }

    public Money Price { get; set; } = Money.Empty;
    
    public InstrumentId InstrumentId { get; set; }

    public static Currency Create(CurrencyId currencyId, string name, FIGI figi, InstrumentId instrumentId)
    {
        var currency = new Currency(currencyId);
        currency.Apply(new CurrencyCreated(currencyId, name, figi, instrumentId));
        return currency;
    }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new CurrencyPriceChanged(Id, newPrice));
    }

    private void When(CurrencyCreated @event)
    {
        Id = @event.CurrencyId;
        Name = @event.Name;
        Figi = @event.Figi;
        InstrumentId = @event.InstrumentId;
    }

    private void When(CurrencyPriceChanged @event)
    {
        Price = @event.NewPrice;
    }
}
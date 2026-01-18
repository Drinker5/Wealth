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

    public InstrumentUId InstrumentUId { get; set; }

    // TODO price currency
    public static Currency Create(CurrencyId currencyId, string name, FIGI figi, InstrumentUId instrumentUId)
    {
        var currency = new Currency(currencyId);
        currency.Apply(new CurrencyCreated(currencyId, name, figi, instrumentUId));
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
        InstrumentUId = @event.InstrumentUId;
    }

    private void When(CurrencyPriceChanged @event)
    {
        Price = @event.NewPrice;
    }

    public void ChangeFigi(FIGI figi)
    {
        if (this.Figi == figi)
            return;

        Apply(new CurrencyFigiChanged(Id, figi));
    }

    public void ChangeInstrumentId(InstrumentUId instrumentUId)
    {
        if (this.InstrumentUId == instrumentUId)
            return;

        Apply(new CurrencyInstrumentIdChanged(Id, instrumentUId));
    }

    public void ChangeName(string name)
    {
        if (this.Name == name)
            return;

        Apply(new CurrencyNameChanged(Id, name));
    }

    private void When(CurrencyFigiChanged @event)
    {
        Figi = @event.Figi;
    }

    private void When(CurrencyInstrumentIdChanged @event)
    {
        InstrumentUId = @event.InstrumentUId;
    }
    
    private void When(CurrencyNameChanged @event)
    {
        Name = @event.Name;
    }
}
using Wealth.BuildingBlocks.Domain;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class StockInstrument : Instrument
{
    private StockInstrument()
    {
    }

    public Dividend Dividend { get; private set; }

    public static StockInstrument Create(string name, ISIN isin, Dividend dividend)
    {
        var stock = new StockInstrument();
        stock.Apply(new StockCreated
        {
            Id = InstrumentId.New(),
            Name = name,
            ISIN = isin,
            Dividend = dividend,
        });
        return stock;
    }

    public void ChangeDividend(Dividend dividend)
    {
        if (this.Dividend == dividend)
            return;
        
        Apply(new DividendChanged(Id, ISIN, dividend));
    }

    private void When(StockCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        ISIN = @event.ISIN;
        Dividend = @event.Dividend;
    }

    private void When(DividendChanged @event)
    {
        Dividend = @event.Dividend;
    }
}

public record DividendChanged(InstrumentId Id, string ISIN, Dividend Dividend) : IDomainEvent;

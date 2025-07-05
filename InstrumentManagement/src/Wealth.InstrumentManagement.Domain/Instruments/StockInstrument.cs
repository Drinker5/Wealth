using Wealth.BuildingBlocks.Domain;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class StockInstrument : Instrument
{
    public StockInstrument(Guid id)
    {
        Id = id;
        Type = InstrumentType.Stock;
    }

    public Dividend Dividend { get; set; }

    public static StockInstrument Create(string name, ISIN isin)
    {
        var instrumentId = InstrumentId.New();
        var stock = new StockInstrument(instrumentId);
        stock.Apply(new StockCreated
        {
            Id = instrumentId,
            Name = name,
            ISIN = isin,
        });
        return stock;
    }

    public void ChangeDividend(Dividend dividend)
    {
        if (this.Dividend == dividend)
            return;

        Apply(new StockDividendChanged
        {
            Id = Id,
            ISIN = ISIN,
            NewDividend = dividend
        });
    }

    private void When(StockCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        ISIN = @event.ISIN;
    }

    private void When(StockDividendChanged @event)
    {
        Dividend = @event.NewDividend;
    }
}
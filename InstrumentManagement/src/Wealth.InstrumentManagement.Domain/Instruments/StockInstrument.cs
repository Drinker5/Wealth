using Wealth.BuildingBlocks.Domain.Common;
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

    public int LotSize { get; set; } = 1;

    public static StockInstrument Create(string name, ISIN isin)
    {
        return Create(InstrumentId.New(), name, isin);
    }

    public static StockInstrument Create(InstrumentId instrumentId, string name, ISIN isin)
    {
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

    public void ChangeLotSize(int lotSize)
    {
        Apply(new StockLotSizeChanged
        {
            Id = Id,
            ISIN = ISIN,
            NewLotSize = lotSize,
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

    private void When(StockLotSizeChanged @event)
    {
        LotSize = @event.NewLotSize;
    }
}
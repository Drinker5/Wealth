using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class Stock(StockId id) : AggregateRoot
{
    public StockId Id { get; private set; } = id;

    public Ticker Ticker { get; set; }
    public string Name { get; set; }

    public ISIN Isin { get; set; }
    public FIGI Figi { get; set; }

    public Money Price { get; set; } = Money.Empty;

    public Dividend Dividend { get; set; } = Dividend.Empty;

    public LotSize LotSize { get; set; } = 1;

    public InstrumentUId InstrumentUId { get; set; }

    public static Stock Create(StockId id, string index, string name, ISIN isin, FIGI figi, InstrumentUId instrumentUId)
    {
        var stock = new Stock(id);
        stock.Apply(new StockCreated(id, index, name, isin, figi, instrumentUId));
        return stock;
    }

    public void ChangeDividend(Dividend dividend)
    {
        if (this.Dividend == dividend)
            return;

        Apply(new StockDividendChanged
        {
            StockId = Id,
            ISIN = Isin,
            NewDividend = dividend
        });
    }

    public void ChangeLotSize(int lotSize)
    {
        Apply(new StockLotSizeChanged
        {
            StockId = Id,
            ISIN = Isin,
            NewLotSize = lotSize,
        });
    }

    public void ChangeTicker(string newIndex)
    {
        Apply(new StockTickerChanged(Id, newIndex));
    }

    private void When(StockCreated @event)
    {
        Id = @event.StockId;
        Name = @event.Name;
        Ticker = @event.Index;
        Isin = @event.Isin;
        Figi = @event.Figi;
        InstrumentUId = @event.InstrumentUId;
    }

    private void When(StockDividendChanged @event)
    {
        Dividend = @event.NewDividend;
    }

    private void When(StockLotSizeChanged @event)
    {
        LotSize = @event.NewLotSize;
    }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new StockPriceChanged
        {
            StockId = Id,
            ISIN = Isin,
            NewPrice = newPrice,
        });
    }

    private void When(StockPriceChanged @event)
    {
        Price = @event.NewPrice;
    }

    private void When(StockTickerChanged @event)
    {
        Ticker = @event.NewTicker;
    }

    public void ChangeIsin(ISIN isin)
    {
        if (this.Isin == isin)
            return;

        Apply(new StockIsinChanged(Id, isin));
    }

    public void ChangeFigi(FIGI figi)
    {
        if (this.Figi == figi)
            return;

        Apply(new StockFigiChanged(Id, figi));
    }

    public void ChangeInstrumentId(InstrumentUId instrumentUId)
    {
        if (this.InstrumentUId == instrumentUId)
            return;

        Apply(new StockInstrumentIdChanged(Id, instrumentUId));
    }

    public void ChangeName(string name)
    {
        if (this.Name == name)
            return;
        
        Apply(new StockNameChanged(Id, name));
    }

    private void When(StockIsinChanged @event)
    {
        Isin = @event.Isin;
    }

    private void When(StockFigiChanged @event)
    {
        Figi = @event.Figi;
    }

    private void When(StockInstrumentIdChanged @event)
    {
        InstrumentUId = @event.InstrumentUId;
    }

    private void When(StockNameChanged @event)
    {
        Name = @event.Name;
    }
}
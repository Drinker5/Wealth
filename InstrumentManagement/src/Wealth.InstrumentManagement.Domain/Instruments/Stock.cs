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

    public InstrumentId InstrumentId { get; set; }

    public static Stock Create(StockId id, string index, string name, ISIN isin, FIGI figi, InstrumentId instrumentId)
    {
        var stock = new Stock(id);
        stock.Apply(new StockCreated(id, index, name, isin, figi, instrumentId));
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
        InstrumentId = @event.InstrumentId;
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

        Apply(new IsinChanged(Id, isin));
    }

    public void ChangeFigi(FIGI figi)
    {
        if (this.Figi == figi)
            return;

        Apply(new FigiChanged(Id, figi));
    }

    public void ChangeInstrumentId(InstrumentId instrumentId)
    {
        if (this.InstrumentId == instrumentId)
            return;

        Apply(new InstrumentIdChanged(Id, instrumentId));
    }

    private void When(IsinChanged @event)
    {
        Isin = @event.Isin;
    }

    private void When(FigiChanged @event)
    {
        Figi = @event.Figi;
    }

    private void When(InstrumentIdChanged @event)
    {
        InstrumentId = @event.InstrumentId;
    }
}
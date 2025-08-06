using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class Stock : AggregateRoot
{
    public StockId Id { get; protected set; }
    
    public string Name { get; set; }

    public ISIN ISIN { get; set; }

    public Money Price { get; set; } = Money.Empty;
    
    private Stock()
    {
    }

    public Dividend Dividend { get; set; } = Dividend.Empty;

    public LotSize LotSize { get; set; } = 1;

    public static Stock Create(string name, ISIN isin)
    {
        return Create(InstrumentId.New(), name, isin);
    }

    public static Stock Create(InstrumentId instrumentId, string name, ISIN isin)
    {
        var stock = new Stock(instrumentId);
        stock.Apply(new StockCreated
        {
            InstrumentId = instrumentId,
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
            InstrumentId = Id,
            ISIN = ISIN,
            NewDividend = dividend
        });
    }

    public void ChangeLotSize(int lotSize)
    {
        Apply(new StockLotSizeChanged
        {
            InstrumentId = Id,
            ISIN = ISIN,
            NewLotSize = lotSize,
        });
    }

    private void When(StockCreated @event)
    {
        Id = @event.InstrumentId;
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
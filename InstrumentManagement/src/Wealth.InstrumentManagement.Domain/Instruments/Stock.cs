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
        return Create(StockId.New(), name, isin);
    }

    public static Stock Create(StockId instrumentId, string name, ISIN isin)
    {
        var stock = new Stock();
        stock.Apply(new StockCreated
        {
            StockId = instrumentId,
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
            StockId = Id,
            ISIN = ISIN,
            NewDividend = dividend
        });
    }

    public void ChangeLotSize(int lotSize)
    {
        Apply(new StockLotSizeChanged
        {
            StockId = Id,
            ISIN = ISIN,
            NewLotSize = lotSize,
        });
    }

    private void When(StockCreated @event)
    {
        Id = @event.StockId;
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

        Apply(new StockPriceChanged
        {
            StockId = Id,
            ISIN = ISIN,
            NewPrice = newPrice,
        });
    }

    private void When(StockPriceChanged @event)
    {
        Price = @event.NewPrice;
    }
}
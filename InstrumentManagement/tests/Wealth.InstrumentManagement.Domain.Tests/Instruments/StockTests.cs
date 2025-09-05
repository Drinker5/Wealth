using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(Stock))]
public class StockTests
{
    readonly string name = "foo";
    readonly ISIN isin = "barbarbarbar";
    readonly Dividend dividend = new Dividend(CurrencyCode.RUB, Decimal.One);

    private Stock CreateStockInstrument(string name, ISIN isin)
    {
        return Stock.Create(3, name, isin);
    }

    [Test]
    public void WhenCreate()
    {
        var stock = CreateStockInstrument(name, isin);

        var @event = stock.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(stock.Id, Is.Not.Zero);
            Assert.That(stock.Name, Is.EqualTo(name));
            Assert.That(stock.ISIN, Is.EqualTo(isin));
            Assert.That(stock.Dividend, Is.EqualTo(Dividend.Empty));
            Assert.That(stock.LotSize.Size, Is.EqualTo(1));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var stock = CreateStockInstrument(name, isin);
        var money = new Money(CurrencyCode.EUR, 23.3m);

        stock.ChangePrice(money);

        var @event = stock.HasEvent<StockPriceChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewPrice, Is.EqualTo(money));
        }
    }

    [Test]
    public void WhenDividendChanged()
    {
        var stock = CreateStockInstrument(name, isin);

        stock.ChangeDividend(dividend);

        var @event = stock.HasEvent<StockDividendChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewDividend, Is.EqualTo(dividend));
        }
    }

    [Test]
    public void WhenLotSizeChanged()
    {
        var instrument = CreateStockInstrument(name, isin);

        instrument.ChangeLotSize(10);

        var @event = instrument.HasEvent<StockLotSizeChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewLotSize, Is.EqualTo(10));
        }
    }
}
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
    readonly Dividend dividend = new Dividend("FOO", Decimal.One);

    private Stock CreateStockInstrument(string name, ISIN isin)
    {
        return Stock.Create(name, isin);
    }

    [Test]
    public void WhenCreate()
    {
        var instrument = CreateStockInstrument(name, isin);

        var @event = instrument.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.InstrumentId, Is.Not.Default);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Id, Is.Not.Default);
            Assert.That(instrument.Name, Is.EqualTo(name));
            Assert.That(instrument.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Dividend, Is.EqualTo(Dividend.Empty));
            Assert.That(instrument.Type, Is.EqualTo(InstrumentType.Stock));
            Assert.That(instrument.LotSize.Size, Is.EqualTo(1));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var instrument = CreateStockInstrument(name, isin);
        var money = new Money("BAR", 23.3m);

        instrument.ChangePrice(money);

        var @event = instrument.HasEvent<InstrumentPriceChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.InstrumentId, Is.Not.Default);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewPrice, Is.EqualTo(money));
        }
    }

    [Test]
    public void WhenDividendChanged()
    {
        var instrument = CreateStockInstrument(name, isin);

        instrument.ChangeDividend(dividend);

        var @event = instrument.HasEvent<StockDividendChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.StockId, Is.Not.Default);
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
            Assert.That(@event.StockId, Is.Not.Default);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewLotSize, Is.EqualTo(10));
        }
    }
}
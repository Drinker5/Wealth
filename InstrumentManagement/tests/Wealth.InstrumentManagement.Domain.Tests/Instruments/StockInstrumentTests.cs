using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(StockInstrument))]
public class StockInstrumentTests
{
    readonly string name = "foo";
    readonly ISIN isin = "barbarbarbar";
    readonly Dividend dividend = new Dividend("FOO", Decimal.One);

    private StockInstrument CreateStockInstrument(string name, ISIN isin)
    {
        return StockInstrument.Create(name, isin);
    }

    [Test]
    public void WhenCreate()
    {
        var instrument = CreateStockInstrument(name, isin);

        var @event = instrument.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.Id, Is.Not.Null);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Id, Is.Not.Default);
            Assert.That(instrument.Name, Is.EqualTo(name));
            Assert.That(instrument.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Dividend, Is.Null);
            Assert.That(instrument.Type, Is.EqualTo(InstrumentType.Stock));
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
            Assert.That(@event.Id, Is.Not.Null);
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
            Assert.That(@event.Id, Is.Not.Null);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewDividend, Is.EqualTo(dividend));
        }
    }
}
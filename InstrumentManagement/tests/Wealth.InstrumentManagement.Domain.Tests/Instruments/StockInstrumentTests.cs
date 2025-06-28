using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(StockInstrument))]
public class StockInstrumentTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void WhenCreate()
    {
        var name = "foo";
        ISIN isin = "barbarbarbar";
        var dividend = new Dividend("FOO", Decimal.One);

        var instrument = StockInstrument.Create(name, isin, dividend);

        var @event = instrument.HasEvent<StockCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.Dividend, Is.EqualTo(dividend));
            Assert.That(instrument.Id, Is.Not.Default);
            Assert.That(instrument.Name, Is.EqualTo(name));
            Assert.That(instrument.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Dividend, Is.EqualTo(dividend));
        }
    }
}
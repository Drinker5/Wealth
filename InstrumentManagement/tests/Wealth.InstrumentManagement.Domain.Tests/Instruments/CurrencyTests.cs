using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(Currency))]
public class CurrencyTests
{
    readonly string name = "foo";
    readonly FIGI figi = "arbarbarbarb";
    readonly InstrumentUId instrumentUId = new Guid("46042ADD-269E-4B54-8848-2004109FF14A");

    private Currency CreateCurrencyInstrument()
    {
        return Currency.Create(3, name, figi, instrumentUId);
    }

    [Test]
    public void WhenCreate()
    {
        var currency = CreateCurrencyInstrument();

        var @event = currency.HasEvent<CurrencyCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.CurrencyId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.InstrumentUId, Is.EqualTo(instrumentUId));
            Assert.That(currency.Id, Is.Not.Zero);
            Assert.That(currency.Name, Is.EqualTo(name));
            Assert.That(currency.InstrumentUId, Is.EqualTo(instrumentUId));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var currency = CreateCurrencyInstrument();
        var money = new Money(CurrencyCode.Eur, 23.3m);

        currency.ChangePrice(money);

        var @event = currency.HasEvent<CurrencyPriceChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.CurrencyId, Is.Not.Zero);
            Assert.That(@event.NewPrice, Is.EqualTo(money));
        }
    }
}
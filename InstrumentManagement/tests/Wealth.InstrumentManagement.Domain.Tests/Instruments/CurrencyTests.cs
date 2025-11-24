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

    private Currency CreateCurrencyInstrument(string name, FIGI figi)
    {
        return Currency.Create(3, name, figi);
    }

    [Test]
    public void WhenCreate()
    {
        var currency = CreateCurrencyInstrument(name, figi);

        var @event = currency.HasEvent<CurrencyCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.CurrencyId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(currency.Id, Is.Not.Zero);
            Assert.That(currency.Name, Is.EqualTo(name));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var currency = CreateCurrencyInstrument(name, figi);
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
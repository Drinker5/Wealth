using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Tests.Instruments;

[TestFixture]
[TestOf(typeof(Bond))]
public class BondTests
{
    readonly string name = "foo";
    readonly ISIN isin = "barbarbarbar";
    readonly Coupon coupon = new Coupon("FOO", Decimal.One);

    private Bond CreateBondInstrument(string name, ISIN isin)
    {
        return Bond.Create(name, isin);
    }

    [Test]
    public void WhenCreate()
    {
        var instrument = CreateBondInstrument(name, isin);

        var @event = instrument.HasEvent<BondCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Default);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Id, Is.Not.Default);
            Assert.That(instrument.Name, Is.EqualTo(name));
            Assert.That(instrument.ISIN, Is.EqualTo(isin));
            Assert.That(instrument.Coupon, Is.Null);
            Assert.That(instrument.Type, Is.EqualTo(InstrumentType.Bond));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var instrument = CreateBondInstrument(name, isin);
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
    public void CouponChanged()
    {
        var instrument = CreateBondInstrument(name, isin);

        instrument.ChangeCoupon(coupon);

        var @event = instrument.HasEvent<BondCouponChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Default);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewCoupon, Is.EqualTo(coupon));
        }
    }
}
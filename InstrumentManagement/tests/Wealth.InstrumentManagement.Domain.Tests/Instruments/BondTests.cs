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
    readonly FIGI figi = "arbarbarbarb";
    readonly Coupon coupon = new Coupon(CurrencyCode.RUB, Decimal.One);

    private Bond CreateBondInstrument(string name, ISIN isin, FIGI figi)
    {
        return Bond.Create(3, name, isin, figi);
    }

    [Test]
    public void WhenCreate()
    {
        var bond = CreateBondInstrument(name, isin, figi);

        var @event = bond.HasEvent<BondCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.Isin, Is.EqualTo(isin));
            Assert.That(bond.Id, Is.Not.Zero);
            Assert.That(bond.Name, Is.EqualTo(name));
            Assert.That(bond.Isin, Is.EqualTo(isin));
            Assert.That(bond.Coupon, Is.Null);
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var bond = CreateBondInstrument(name, isin, figi);
        var money = new Money(CurrencyCode.EUR, 23.3m);

        bond.ChangePrice(money);

        var @event = bond.HasEvent<BondPriceChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewPrice, Is.EqualTo(money));
        }
    }

    [Test]
    public void CouponChanged()
    {
        var bond = CreateBondInstrument(name, isin, figi);

        bond.ChangeCoupon(coupon);

        var @event = bond.HasEvent<BondCouponChanged>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Zero);
            Assert.That(@event.ISIN, Is.EqualTo(isin));
            Assert.That(@event.NewCoupon, Is.EqualTo(coupon));
        }
    }
}
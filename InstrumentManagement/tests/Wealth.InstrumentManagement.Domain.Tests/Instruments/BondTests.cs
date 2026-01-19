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
    readonly InstrumentUId instrumentUId = new Guid("2E7B63F8-EE1F-4007-850A-F52194B34476");
    readonly Coupon coupon = new Coupon(CurrencyCode.Rub, Decimal.One);

    private Bond CreateBondInstrument()
    {
        return Bond.Create(3, name, isin, figi, instrumentUId, CurrencyCode.Rub);
    }

    [Test]
    public void WhenCreate()
    {
        var bond = CreateBondInstrument();

        var @event = bond.HasEvent<BondCreated>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(@event.BondId, Is.Not.Zero);
            Assert.That(@event.Name, Is.EqualTo(name));
            Assert.That(@event.Isin, Is.EqualTo(isin));
            Assert.That(@event.InstrumentUId, Is.EqualTo(instrumentUId));
            Assert.That(bond.Id, Is.Not.Zero);
            Assert.That(bond.Name, Is.EqualTo(name));
            Assert.That(bond.Isin, Is.EqualTo(isin));
            Assert.That(bond.InstrumentUId, Is.EqualTo(instrumentUId));
            Assert.That(bond.Coupon, Is.EqualTo(Coupon.Empty));
        }
    }

    [Test]
    public void WhenPriceChanged()
    {
        var bond = CreateBondInstrument();
        var money = new Money(CurrencyCode.Eur, 23.3m);

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
        var bond = CreateBondInstrument();

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
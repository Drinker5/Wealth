using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class Bond(BondId id) : AggregateRoot
{
    public BondId Id { get; private set; } = id;

    public string Name { get; set; }

    public ISIN Isin { get; set; }
    public FIGI Figi { get; set; }
    public InstrumentUId InstrumentUId { get; set; }

    public Money Price { get; set; } = Money.Empty;

    public Coupon Coupon { get; set; }

    public static Bond Create(BondId bondId, string name, ISIN isin, FIGI figi, InstrumentUId instrumentUId)
    {
        var bond = new Bond(bondId);
        bond.Apply(new BondCreated
        {
            BondId = bondId,
            Name = name,
            Isin = isin,
            Figi = figi,
            InstrumentUId = instrumentUId
        });
        return bond;
    }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new BondPriceChanged
        {
            BondId = Id,
            ISIN = Isin,
            NewPrice = newPrice,
        });
    }

    private void When(BondCreated @event)
    {
        Id = @event.BondId;
        Name = @event.Name;
        Isin = @event.Isin;
        Figi = @event.Figi;
        InstrumentUId = @event.InstrumentUId;
    }

    public void ChangeCoupon(Coupon coupon)
    {
        if (Coupon == coupon)
            return;

        Apply(new BondCouponChanged
        {
            BondId = Id,
            ISIN = Isin,
            NewCoupon = coupon
        });
    }

    private void When(BondCouponChanged @event)
    {
        Coupon = @event.NewCoupon;
    }

    private void When(BondPriceChanged @event)
    {
        Price = @event.NewPrice;
    }

    public void ChangeIsin(ISIN isin)
    {
        if (this.Isin == isin)
            return;

        Apply(new BondIsinChanged(Id, isin));
    }

    public void ChangeFigi(FIGI figi)
    {
        if (this.Figi == figi)
            return;

        Apply(new BondFigiChanged(Id, figi));
    }

    public void ChangeInstrumentId(InstrumentUId instrumentUId)
    {
        if (this.InstrumentUId == instrumentUId)
            return;

        Apply(new BondInstrumentIdChanged(Id, instrumentUId));
    }

    public void ChangeName(string name)
    {
        if (this.Name == name)
            return;
        
        Apply(new BondNameChanged(Id, name));
    }

    private void When(BondIsinChanged @event)
    {
        Isin = @event.Isin;
    }

    private void When(BondFigiChanged @event)
    {
        Figi = @event.Figi;
    }

    private void When(BondInstrumentIdChanged @event)
    {
        InstrumentUId = @event.InstrumentUId;
    }
    
    private void When(BondNameChanged @event)
    {
        Name = @event.Name;
    }
}
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class BondInstrument : Instrument
{
    private BondInstrument()
    {
        Type = InstrumentType.Bond;
    }

    public Coupon Coupon { get; private set; }

    public static BondInstrument Create(string name, ISIN isin)
    {
        var bond = new BondInstrument();
        bond.Apply(new BondCreated
        {
            Id = InstrumentId.New(),
            Name = name,
            ISIN = isin,
        });
        return bond;
    }
    
    private void When(BondCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        ISIN = @event.ISIN;
    }

    public void ChangeCoupon(Coupon coupon)
    {
        if (Coupon == coupon)
            return;
        
        Apply(new BondCouponChanged
        {
            Id = Id,
            ISIN = ISIN,
            NewCoupon = coupon
        });
    }

    private void When(BondCouponChanged @event)
    {
        Coupon = @event.NewCoupon;
    }
}
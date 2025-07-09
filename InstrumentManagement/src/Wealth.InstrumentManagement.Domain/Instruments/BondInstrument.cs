using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class BondInstrument : Instrument
{
    public BondInstrument(Guid id)
    {
        this.Id = id;
        Type = InstrumentType.Bond;
    }

    public Coupon Coupon { get; set; }

    public static BondInstrument Create(string name, ISIN isin)
    {
        return Create(InstrumentId.New(), name, isin);
    }
    
    public static BondInstrument Create(InstrumentId instrumentId, string name, ISIN isin)
    {
        var bond = new BondInstrument(instrumentId);
        bond.Apply(new BondCreated
        {
            Id = instrumentId,
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
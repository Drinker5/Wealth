using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class BondInstrument : Instrument
{
    private BondInstrument()
    {
    }

    public Coupon Coupon { get; private set; }

    public static BondInstrument Create(string name, ISIN isin, Coupon coupon)
    {
        var bond = new BondInstrument();
        bond.Apply(new BondCreated
        {
            Id = InstrumentId.New(),
            Name = name,
            ISIN = isin,
            Coupon = coupon,
        });
        return bond;
    }
    
    private void When(BondCreated @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        ISIN = @event.ISIN;
        Coupon = @event.Coupon;
    }
}
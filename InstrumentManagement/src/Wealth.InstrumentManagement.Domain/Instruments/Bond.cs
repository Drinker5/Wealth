using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments.Events;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public class Bond : AggregateRoot
{
    public BondId Id { get; protected set; }
    
    public string Name { get; set; }

    public ISIN ISIN { get; set; }

    public Money Price { get; set; } = Money.Empty;
    
    public Bond(Guid id)
    {
        this.Id = id;
        Type = InstrumentType.Bond;
    }

    public Coupon Coupon { get; set; }

    public static Bond Create(string name, ISIN isin)
    {
        return Create(InstrumentId.New(), name, isin);
    }
    
    public static Bond Create(InstrumentId instrumentId, string name, ISIN isin)
    {
        var bond = new Bond(instrumentId);
        bond.Apply(new BondCreated
        {
            InstrumentId = instrumentId,
            Name = name,
            ISIN = isin,
        });
        return bond;
    }

    public void ChangePrice(Money newPrice)
    {
        if (Price == newPrice)
            return;

        Apply(new InstrumentPriceChanged
        {
            InstrumentId = Id,
            ISIN = ISIN,
            NewPrice = newPrice,
            Type = Type,
        });
    }

    private void When(BondCreated @event)
    {
        Id = @event.InstrumentId;
        Name = @event.Name;
        ISIN = @event.ISIN;
    }

    public void ChangeCoupon(Coupon coupon)
    {
        if (Coupon == coupon)
            return;
        
        Apply(new BondCouponChanged
        {
            InstrumentId = Id,
            ISIN = ISIN,
            NewCoupon = coupon
        });
    }

    private void When(BondCouponChanged @event)
    {
        Coupon = @event.NewCoupon;
    }

    private void When(InstrumentPriceChanged @event)
    {
        Price = @event.NewPrice;
    }
}
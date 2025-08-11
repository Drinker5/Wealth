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
    
    public Bond(BondId Id)
    {
        Id = Id;
    }

    public Coupon Coupon { get; set; }

    public static Bond Create(string name, ISIN isin)
    {
        return Create(BondId.New(), name, isin);
    }
    
    public static Bond Create(BondId bondId, string name, ISIN isin)
    {
        var bond = new Bond(bondId);
        bond.Apply(new BondCreated
        {
            BondId = bondId,
            Name = name,
            ISIN = isin,
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
            ISIN = ISIN,
            NewPrice = newPrice,
        });
    }

    private void When(BondCreated @event)
    {
        Id = @event.BondId;
        Name = @event.Name;
        ISIN = @event.ISIN;
    }

    public void ChangeCoupon(Coupon coupon)
    {
        if (Coupon == coupon)
            return;
        
        Apply(new BondCouponChanged
        {
            BondId = Id,
            ISIN = ISIN,
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
}
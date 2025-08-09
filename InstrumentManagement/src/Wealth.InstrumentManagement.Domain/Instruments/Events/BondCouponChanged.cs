using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record BondCouponChanged : DomainEvent
{
    public BondId BondId { get; set; }
    public ISIN ISIN { get; set; }
    public Coupon NewCoupon { get; set; }
}
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Events;

public record BondCouponChangedIntegrationEvent : IntegrationEvent
{
    public InstrumentId InstrumentId { get; set; }
    public ISIN ISIN { get; set; }
    public Coupon NewCoupon { get; set; }
}
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCouponCommand : ICommand
{
    public InstrumentId Id { get; set; }
    public Coupon Coupon { get; set; }
}
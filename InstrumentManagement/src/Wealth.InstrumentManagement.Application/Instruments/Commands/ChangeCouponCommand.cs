using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct ChangeCouponCommand(BondId BondId, Coupon Coupon) : ICommand;

public class ChangeCouponCommandHandler(IBondsRepository repository) : ICommandHandler<ChangeCouponCommand>
{
    public async Task Handle(ChangeCouponCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangeCoupon(request.BondId, request.Coupon);
    }
}
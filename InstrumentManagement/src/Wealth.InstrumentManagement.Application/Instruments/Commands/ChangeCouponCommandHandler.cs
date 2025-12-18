using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCouponCommandHandler(IBondsRepository repository) : ICommandHandler<ChangeCouponCommand>
{
    public async Task Handle(ChangeCouponCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangeCoupon(request.Id, request.Coupon);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCouponCommandHandler : ICommandHandler<ChangeCouponCommand>
{
    private readonly IBondsRepository repository;
    private readonly ICurrencyService currencyService;

    public ChangeCouponCommandHandler(IBondsRepository repository, ICurrencyService currencyService)
    {
        this.repository = repository;
        this.currencyService = currencyService;
    }

    public async Task Handle(ChangeCouponCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Coupon.ValuePerYear.CurrencyId))
            return;
        
        await repository.ChangeCoupon(request.Id, request.Coupon);
    }
}
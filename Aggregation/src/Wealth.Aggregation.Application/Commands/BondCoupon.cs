using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record BondCoupon(    
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    BondId BondId,
    Money Amount) : ICommand;
    
public class BondCouponHandler(IBondCouponRepository repository) : ICommandHandler<BondCoupon>
{
    public async Task Handle(BondCoupon command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

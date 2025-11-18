using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record BondCurrencyOperation(    
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    BondId BondId,
    Money Amount,
    BondCurrencyOperationType Type) : ICommand;
    
public class BondCurrencyOperationHandler(IBondCurrencyOperationRepository repository) : ICommandHandler<BondCurrencyOperation>
{
    public async Task Handle(BondCurrencyOperation command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

public enum BondCurrencyOperationType : byte
{
    Coupon = 1,
    CouponTax = 2,
    BrokerFee = 3,
}
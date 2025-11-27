using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record BondMoneyOperation(    
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    BondId BondId,
    Money Amount,
    BondMoneyOperationType Type) : ICommand;
    
public class BondMoneyOperationHandler(IBondMoneyOperationRepository repository) : ICommandHandler<BondMoneyOperation>
{
    public async Task Handle(BondMoneyOperation command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

public enum BondMoneyOperationType : byte
{
    Coupon = 1,
    CouponTax = 2,
    BrokerFee = 3,
}
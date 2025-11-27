using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record CurrencyMoneyOperation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    CurrencyId CurrencyId,
    Money Amount,
    CurrencyMoneyOperationType Type) : ICommand;

public class CurrencyMoneyOperationHandler(ICurrencyMoneyOperationRepository repository) : ICommandHandler<CurrencyMoneyOperation>
{
    public async Task Handle(CurrencyMoneyOperation command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

public enum CurrencyMoneyOperationType : byte
{
    BrokerFee = 1,
}
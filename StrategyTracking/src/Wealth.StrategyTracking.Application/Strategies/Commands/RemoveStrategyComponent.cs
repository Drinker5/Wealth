using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record struct RemoveStockStrategyComponent(StrategyId StrategyId, StockId InstrumentId) : ICommand;

public record struct RemoveBondStrategyComponent(StrategyId StrategyId, BondId InstrumentId) : ICommand;

public record struct RemoveCurrencyStrategyComponent(StrategyId StrategyId, CurrencyId InstrumentId) : ICommand;

public class RemoveStrategyComponentCommandHandler(IStrategyRepository repository) :
    ICommandHandler<RemoveStockStrategyComponent>,
    ICommandHandler<RemoveBondStrategyComponent>,
    ICommandHandler<RemoveCurrencyStrategyComponent>
{
    public Task Handle(RemoveStockStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId, cancellationToken);
    }

    public Task Handle(RemoveBondStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId, cancellationToken);
    }

    public Task Handle(RemoveCurrencyStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId, cancellationToken);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class RemoveStrategyComponentCommandHandler(IStrategyRepository repository) :
    ICommandHandler<RemoveStockStrategyComponent>,
    ICommandHandler<RemoveBondStrategyComponent>,
    ICommandHandler<RemoveCurrencyStrategyComponent>
{
    public Task Handle(RemoveStockStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId);
    }

    public Task Handle(RemoveBondStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId);
    }

    public Task Handle(RemoveCurrencyStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId);
    }
}
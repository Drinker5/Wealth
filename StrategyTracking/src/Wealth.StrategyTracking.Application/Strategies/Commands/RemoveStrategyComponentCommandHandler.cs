using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class RemoveStrategyComponentCommandHandler(IStrategyRepository repository) : ICommandHandler<RemoveStrategyComponent>
{
    public Task Handle(RemoveStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.RemoveStrategyComponent(request.StrategyId, request.InstrumentId);
    }
}
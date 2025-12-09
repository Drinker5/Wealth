using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record struct ChangeMasterStrategy(StrategyId StrategyId, MasterStrategy MasterStrategy) : ICommand;

public class ChangeMasterStrategyHandler(IStrategyRepository repository) : ICommandHandler<ChangeMasterStrategy>
{
    public Task Handle(ChangeMasterStrategy request, CancellationToken cancellationToken)
    {
        return repository.ChangeMasterStrategy(request.StrategyId, request.MasterStrategy, cancellationToken);
    }
}
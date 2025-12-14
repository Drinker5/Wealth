using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record struct UpdateStrategy(StrategyId StrategyId) : ICommand;

public class UpdateStrategyCommandHandler(
    IStrategyRepository repository,
    ComponentsProviderFactory componentsProviderFactory) : ICommandHandler<UpdateStrategy>
{
    public async Task Handle(UpdateStrategy request, CancellationToken cancellationToken)
    {
        var strategy = await repository.GetStrategy(request.StrategyId, cancellationToken);
        if (strategy == null)
            throw new ApplicationException($"Strategy {request.StrategyId} does not exist.");

        if (strategy.FollowedStrategy == MasterStrategy.None)
            return;

        var provider = componentsProviderFactory.GetComponentsProvider(strategy.FollowedStrategy);
        var components = await provider.GetComponents(cancellationToken);
        strategy.SetComponents(components);
    }
}
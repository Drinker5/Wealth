using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class CreateStrategyCommandHandler(IStrategyRepository repository) : ICommandHandler<CreateStrategy, StrategyId>
{
    public Task<StrategyId> Handle(CreateStrategy request, CancellationToken cancellationToken)
    {
        return repository.CreateStrategy(request.Name);
    }
}
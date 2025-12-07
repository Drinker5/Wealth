using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class CreateStrategyCommandHandler(IStrategyRepository repository) : ICommandHandler<CreateStrategy, StrategyId>
{
    public Task<StrategyId> Handle(CreateStrategy request, CancellationToken cancellationToken)
    {
        return repository.CreateStrategy(request.Name);
    }
}
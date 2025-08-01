using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class RenameStrategyCommandHandler(IStrategyRepository repository) : ICommandHandler<RenameStrategy>
{
    public Task Handle(RenameStrategy request, CancellationToken cancellationToken)
    {
        return repository.RenameStrategy(request.StrategyId, request.NewName);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class AddStrategyComponentCommandHandler(IStrategyRepository repository) : ICommandHandler<AddStrategyComponent>
{
    public Task Handle(AddStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class ChangeStrategyComponentWeightCommandHandler(IStrategyRepository repository) : ICommandHandler<ChangeStrategyComponentWeight>
{
    public Task Handle(ChangeStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class ChangeStrategyComponentWeightCommandHandler(IStrategyRepository repository) :
    ICommandHandler<ChangeStockStrategyComponentWeight>,
    ICommandHandler<ChangeBondStrategyComponentWeight>,
    ICommandHandler<ChangeCurrencyStrategyComponentWeight>
{
    public Task Handle(ChangeStockStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight);
    }

    public Task Handle(ChangeBondStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight);
    }

    public Task Handle(ChangeCurrencyStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight);
    }
}
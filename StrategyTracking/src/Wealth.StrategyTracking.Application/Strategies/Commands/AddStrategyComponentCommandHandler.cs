using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public class AddStrategyComponentCommandHandler(IStrategyRepository repository) :
    ICommandHandler<AddStockStrategyComponent>,
    ICommandHandler<AddBondStrategyComponent>,
    ICommandHandler<AddCurrencyStrategyComponent>
{
    public Task Handle(AddStockStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight);
    }

    public Task Handle(AddBondStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight);
    }

    public Task Handle(AddCurrencyStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight);
    }
}
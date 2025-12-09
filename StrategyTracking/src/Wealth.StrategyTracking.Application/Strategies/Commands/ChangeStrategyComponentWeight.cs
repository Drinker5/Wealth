using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record struct ChangeStockStrategyComponentWeight(StrategyId StrategyId, StockId InstrumentId, float Weight) : ICommand;
public record struct ChangeBondStrategyComponentWeight(StrategyId StrategyId, BondId InstrumentId, float Weight) : ICommand;
public record struct ChangeCurrencyStrategyComponentWeight(StrategyId StrategyId, CurrencyId InstrumentId, float Weight) : ICommand;

public class ChangeStrategyComponentWeightCommandHandler(IStrategyRepository repository) :
    ICommandHandler<ChangeStockStrategyComponentWeight>,
    ICommandHandler<ChangeBondStrategyComponentWeight>,
    ICommandHandler<ChangeCurrencyStrategyComponentWeight>
{
    public Task Handle(ChangeStockStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }

    public Task Handle(ChangeBondStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }

    public Task Handle(ChangeCurrencyStrategyComponentWeight request, CancellationToken cancellationToken)
    {
        return repository.ChangeStrategyComponentWeight(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }
}
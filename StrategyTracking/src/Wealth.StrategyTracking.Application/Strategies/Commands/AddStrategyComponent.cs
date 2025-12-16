using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Repositories;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record struct AddStockStrategyComponent(StrategyId StrategyId, StockId InstrumentId, decimal Weight) : ICommand;

public record struct AddBondStrategyComponent(StrategyId StrategyId, BondId InstrumentId, decimal Weight) : ICommand;

public record struct AddCurrencyStrategyComponent(StrategyId StrategyId, CurrencyId InstrumentId, decimal Weight) : ICommand;

public class AddStrategyComponentCommandHandler(IStrategyRepository repository) :
    ICommandHandler<AddStockStrategyComponent>,
    ICommandHandler<AddBondStrategyComponent>,
    ICommandHandler<AddCurrencyStrategyComponent>
{
    public Task Handle(AddStockStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }

    public Task Handle(AddBondStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }

    public Task Handle(AddCurrencyStrategyComponent request, CancellationToken cancellationToken)
    {
        return repository.AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight, cancellationToken);
    }
}
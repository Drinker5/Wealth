using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record CreateStrategy(string Name) : ICommand<StrategyId>;

public record RenameStrategy(StrategyId StrategyId, string NewName) : ICommand;

public record AddStockStrategyComponent(StrategyId StrategyId, StockId InstrumentId, float Weight) : ICommand;

public record AddBondStrategyComponent(StrategyId StrategyId, BondId InstrumentId, float Weight) : ICommand;

public record AddCurrencyStrategyComponent(StrategyId StrategyId, CurrencyId InstrumentId, float Weight) : ICommand;

public record RemoveStockStrategyComponent(StrategyId StrategyId, StockId InstrumentId) : ICommand;

public record RemoveBondStrategyComponent(StrategyId StrategyId, BondId InstrumentId) : ICommand;

public record RemoveCurrencyStrategyComponent(StrategyId StrategyId, CurrencyId InstrumentId) : ICommand;

public record ChangeStockStrategyComponentWeight(StrategyId StrategyId, StockId InstrumentId, float Weight) : ICommand;

public record ChangeBondStrategyComponentWeight(StrategyId StrategyId, BondId InstrumentId, float Weight) : ICommand;

public record ChangeCurrencyStrategyComponentWeight(StrategyId StrategyId, CurrencyId InstrumentId, float Weight) : ICommand;
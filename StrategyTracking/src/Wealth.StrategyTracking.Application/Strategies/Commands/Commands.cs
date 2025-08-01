using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Commands;

public record CreateStrategy(string Name) : ICommand<StrategyId>;

public record RenameStrategy(StrategyId StrategyId, string NewName) : ICommand;

public record AddStrategyComponent(StrategyId StrategyId, InstrumentId InstrumentId, float Weight) : ICommand;

public record RemoveStrategyComponent(StrategyId StrategyId, InstrumentId InstrumentId) : ICommand;

public record ChangeStrategyComponentWeight(StrategyId StrategyId, InstrumentId InstrumentId, float Weight) : ICommand;
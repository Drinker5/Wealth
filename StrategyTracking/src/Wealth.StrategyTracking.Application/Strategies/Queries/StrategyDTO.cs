using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public class StrategyDTO
{
    public StrategyId StrategyId { get; set; }
    public string Name { get; set; }
    public IReadOnlyCollection<StrategyComponent> Components { get; set; }
}
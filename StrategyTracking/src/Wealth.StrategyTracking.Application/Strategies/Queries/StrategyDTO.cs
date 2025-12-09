using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.Queries;

public class StrategyDTO
{
    public StrategyId StrategyId { get; set; }
    public string Name { get; set; }
    public MasterStrategy FollowedStrategy { get; set; }
    public IReadOnlyCollection<StrategyComponent> Components { get; set; }
    
    public static StrategyDTO Convert(Strategy s)
    {
        return new StrategyDTO
        {
            StrategyId = s.Id,
            Name = s.Name,
            FollowedStrategy = s.FollowedStrategy,
            Components = s.Components,
        };
    }
}
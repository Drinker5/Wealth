using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;

public interface IComponentsProvider
{
    Task<IReadOnlyList<StrategyComponent>> GetComponents(CancellationToken token);
}
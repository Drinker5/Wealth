using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;

public interface IComponentsProvider
{
    Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token);
}
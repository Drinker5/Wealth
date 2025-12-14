using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;

public sealed class ComponentsProviderFactory(
    IMoexComponentsProvider moexComponentsProvider)
{
    public IComponentsProvider GetComponentsProvider(MasterStrategy followedStrategy)
        => followedStrategy switch
        {
            MasterStrategy.IMOEX => moexComponentsProvider,
            _ => throw new ArgumentOutOfRangeException(nameof(followedStrategy), followedStrategy, null)
        };
}
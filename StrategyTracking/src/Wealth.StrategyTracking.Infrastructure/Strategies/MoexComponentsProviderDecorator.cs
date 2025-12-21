using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Infrastructure.Strategies;

public sealed class MoexComponentsProviderDecorator(
    IMoexComponentsProvider iMoexComponentsProvider) : IMoexComponentsProvider
{
    private IReadOnlyCollection<StrategyComponent>? cache;

    public async Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        if (cache != null)
            return cache;

        cache ??= await iMoexComponentsProvider.GetComponents(token);
        return cache;
    }
}
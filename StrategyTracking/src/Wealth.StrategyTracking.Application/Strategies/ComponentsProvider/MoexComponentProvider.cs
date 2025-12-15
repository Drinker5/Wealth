using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;

public sealed class MoexComponentProvider : IMoexComponentsProvider
{
    public Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
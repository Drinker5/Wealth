using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Application.Strategies.ComponentsProvider;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.API.Tests;

public sealed class TestMoexComponentsProvider : IMoexComponentsProvider
{
    public static readonly IReadOnlyCollection<StrategyComponent> Components =
    [
        new StockStrategyComponent
        {
            StockId = 1,
            Weight = 0.40f
        },
        new BondStrategyComponent
        {
            BondId = 2,
            Weight = 0.30f
        },
        new CurrencyAssetStrategyComponent
        {
            CurrencyId = 3,
            Weight = 0.20f
        },
        new CurrencyStrategyComponent
        {
            Currency = CurrencyCode.Usd,
            Weight = 0.10f
        }
    ];

    public Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        return Task.FromResult(Components);
    }
}
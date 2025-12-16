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
            Weight = 40
        },
        new BondStrategyComponent
        {
            BondId = 2,
            Weight = 30
        },
        new CurrencyAssetStrategyComponent
        {
            CurrencyId = 3,
            Weight = 20
        },
        new CurrencyStrategyComponent
        {
            Currency = CurrencyCode.Usd,
            Weight = 10
        }
    ];

    public Task<IReadOnlyCollection<StrategyComponent>> GetComponents(CancellationToken token)
    {
        return Task.FromResult(Components);
    }
}
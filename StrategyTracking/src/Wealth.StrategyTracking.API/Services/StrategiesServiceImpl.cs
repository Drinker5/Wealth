using Grpc.Core;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Application;
using Wealth.StrategyTracking.Application.Strategies.Queries;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.API.Services;

public sealed class StrategiesServiceImpl(ICqrsInvoker mediator) : StrategiesService.StrategiesServiceBase
{
    public override async Task<GetStrategyResponse> GetStrategy(GetStrategyRequest request, ServerCallContext context)
    {
        var strategy = await mediator.Query(new GetStrategy(request.StrategyId));
        if (strategy == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Strategy not found"));

        return new GetStrategyResponse
        {
            Name = strategy.Name,
            Components = { strategy.Components.Select(ToProto) }
        };
    }

    private static GetStrategyResponse.Types.StrategyComponent ToProto(StrategyComponent strategyComponent)
    {
        return new GetStrategyResponse.Types.StrategyComponent
        {
            InstrumentId = strategyComponent.Id,
            InstrumentType = strategyComponent switch
            {
                StockStrategyComponent => InstrumentTypeProto.Stock,
                BondStrategyComponent => InstrumentTypeProto.Bond,
                CurrencyAssetStrategyComponent => InstrumentTypeProto.CurrencyAsset,
                CurrencyStrategyComponent => InstrumentTypeProto.Currency,
                _ => throw new ArgumentOutOfRangeException(nameof(strategyComponent))
            },
            Weight = strategyComponent.Weight,
        };
    }
}
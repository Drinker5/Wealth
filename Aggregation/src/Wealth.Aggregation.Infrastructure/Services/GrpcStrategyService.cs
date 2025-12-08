using Grpc.Core;
using Wealth.Aggregation.Application.Services;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking;

namespace Wealth.Aggregation.Infrastructure.Services;

public sealed class GrpcStrategyService(StrategiesService.StrategiesServiceClient client) : IStrategyService
{
    public async Task<Strategy?> GetStrategy(StrategyId strategyId, CancellationToken token)
    {
        try
        {
            var response = await client.GetStrategyAsync(
                new GetStrategyRequest { StrategyId = strategyId },
                cancellationToken: token);

            var components = response.Components
                .Select(i => new StrategyComponent(i.InstrumentId, FromProto(i.InstrumentType), i.Weight))
                .ToArray();

            return new Strategy(strategyId, response.Name, components);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    private static InstrumentType FromProto(InstrumentTypeProto instrumentType) =>
        instrumentType switch
        {
            InstrumentTypeProto.Stock => InstrumentType.Stock,
            InstrumentTypeProto.Bond => InstrumentType.Bond,
            InstrumentTypeProto.CurrencyAsset => InstrumentType.CurrencyAsset,
            InstrumentTypeProto.Currency => InstrumentType.Currency,
            _ => throw new ArgumentOutOfRangeException(nameof(instrumentType), instrumentType, null)
        };
}
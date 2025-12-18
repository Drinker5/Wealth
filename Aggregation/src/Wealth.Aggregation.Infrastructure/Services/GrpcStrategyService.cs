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
                .Select(i => new StrategyComponent(
                    i.InstrumentId,
                    i.InstrumentType.FromProto(),
                    i.Weight))
                .ToArray();

            return new Strategy(strategyId, response.Name, components);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}
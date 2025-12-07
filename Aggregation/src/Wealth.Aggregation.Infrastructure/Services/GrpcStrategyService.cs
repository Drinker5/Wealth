using Grpc.Core;
using Wealth.Aggregation.Application.Services;
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
            
            return new Strategy(strategyId, response.Name);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }
}

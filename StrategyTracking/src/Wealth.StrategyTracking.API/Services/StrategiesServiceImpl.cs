using Grpc.Core;
using Wealth.InstrumentManagement;

namespace Wealth.StrategyTracking.API.Services;

public class StrategiesServiceImpl : StrategiesService.StrategiesServiceBase
{
    public override Task<GetStrategyResponse> GetStrategy(GetStrategyRequest request, ServerCallContext context)
    {
        throw new NotImplementedException();
    }
}
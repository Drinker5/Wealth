using Microsoft.AspNetCore.Http.HttpResults;
using Wealth.StrategyTracking.Application.Strategies.Queries;

namespace Wealth.StrategyTracking.API.APIs;

public static class StrategyTrackingApi
{
    public static RouteGroupBuilder MapWalletApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/strategies");

        api.MapGet("/", GetStrategies);
        return api;
    }

    private static async Task<Results<Ok<IEnumerable<StrategyDTO>>, ProblemHttpResult>> GetStrategies(
        [AsParameters] StrategyTrackingServices services)
    {
        var result = await services.Mediator.Query(new GetStrategies());
        return TypedResults.Ok(result);
    }
}
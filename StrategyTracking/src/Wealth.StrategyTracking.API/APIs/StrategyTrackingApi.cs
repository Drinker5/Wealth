using Microsoft.AspNetCore.Http.HttpResults;

namespace Wealth.StrategyTracking.API.APIs;

public static class StrategyTrackingApi
{
    public static RouteGroupBuilder MapWalletApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/strategy");

        api.MapGet("/", Get);
        return api;
    }

    private static Ok<string> Get(
        [AsParameters] StrategyTrackingServices services)
    {
        return TypedResults.Ok("ok");
    }
}
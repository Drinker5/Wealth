using Microsoft.AspNetCore.Http.HttpResults;
using Wealth.InstrumentManagement.Application;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.API.APIs;

public static class InstrumentsApi
{
    public static RouteGroupBuilder MapPortfolioApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/instruments");
        api.MapPost("/update-prices", UpdatePrices);
        return api;
    }

    private static async Task<Ok> UpdatePrices(
        IPriceUpdater provider,
        CancellationToken token)
    {
        await provider.UpdatePrices(token);
        return TypedResults.Ok();
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;

namespace Wealth.InstrumentManagement.API.APIs;

public static class InstrumentsApi
{
    public static RouteGroupBuilder MapInstrumentsApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/instruments");
        api.MapPost("/update-prices", UpdatePrices);
        api.MapPost("/import-instruments", ImportInstruments);
        return api;
    }

    private static async Task<Ok> UpdatePrices(
        IPriceUpdater provider,
        CancellationToken token)
    {
        await provider.UpdatePrices(token);
        return TypedResults.Ok();
    }

    private static async Task<Ok> ImportInstruments(
        ICqrsInvoker cqrsInvoker,
        [FromBody] ImportInstruments command,
        CancellationToken token)
    {
        await cqrsInvoker.Command(command, token);
        return TypedResults.Ok();
    }
}
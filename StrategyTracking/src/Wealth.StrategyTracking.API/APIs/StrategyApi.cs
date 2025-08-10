using Microsoft.AspNetCore.Http.HttpResults;
using Wealth.StrategyTracking.Application.Strategies.Commands;
using Wealth.StrategyTracking.Application.Strategies.Queries;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.API.APIs;

public static class StrategyApi
{
    public static RouteGroupBuilder MapStrategyApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/strategy");

        api.MapGet("{strategyId:int}", GetStrategy);
        api.MapPost("/", CreateStrategy);
        api.MapPut("/add-component", AddStrategyComponent);
        return api;
    }

    private static async Task<Results<Ok, BadRequest<string>>> AddStrategyComponent(
        AddStrategyComponentRequest request,
        [AsParameters] StrategyTrackingServices services)
    {
        try
        {
            await services.Mediator.Command(new AddStrategyComponent(request.StrategyId, request.InstrumentId, request.Weight));
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok<StrategyDTO>, NotFound>> GetStrategy(
        int strategyId,
        [AsParameters] StrategyTrackingServices services)
    {
        var result = await services.Mediator.Query(new GetStrategy(strategyId));
        if (result == null)
            return TypedResults.NotFound();
        
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<int>, BadRequest<string>>> CreateStrategy(
        CreateStrategyRequest request,
        [AsParameters] StrategyTrackingServices services)
    {
        try
        {
            var strategyId = await services.Mediator.Command(new CreateStrategy(request.Name));
            return TypedResults.Ok(strategyId.Id);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}

internal record AddStrategyComponentRequest(StrategyId StrategyId, InstrumentId InstrumentId, float Weight);

internal record struct CreateStrategyRequest(string Name);

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Domain.Common;
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
        api.MapPost("/follow", ChangeMasterStrategy);
        api.MapPost("/update", UpdateStrategy);
        api.MapPut("/add-stock-component", AddStockStrategyComponent);
        return api;
    }

    private static async Task<Results<Ok, BadRequest<string>>> AddStockStrategyComponent(
        AddStockStrategyComponentRequest request,
        [AsParameters] StrategyTrackingServices services)
    {
        try
        {
            await services.Mediator.Command(new AddStockStrategyComponent(request.StrategyId, request.StockId, request.Weight));
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
            return TypedResults.Ok(strategyId.Value);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok, BadRequest<string>>> ChangeMasterStrategy(
        ChangeMasterStrategyRequest request,
        [AsParameters] StrategyTrackingServices services)
    {
        try
        {
            await services.Mediator.Command(new ChangeMasterStrategy(request.StrategyId, request.MasterStrategy));
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok, BadRequest<string>>> UpdateStrategy(
        StrategyId strategyId,
        [AsParameters] StrategyTrackingServices services)
    {
        try
        {
            await services.Mediator.Command(new UpdateStrategy(strategyId));
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    } 
}

internal record struct AddStockStrategyComponentRequest(StrategyId StrategyId, StockId StockId, decimal Weight);

internal record struct CreateStrategyRequest(string Name);

internal record struct ChangeMasterStrategyRequest(int StrategyId, MasterStrategy MasterStrategy);
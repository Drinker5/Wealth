using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Application.Portfolios.Queries;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.API.APIs;

public static class PortfolioApi
{
    public static RouteGroupBuilder MapPortfolioApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/portfolio");

        api.MapGet("/", GetPortfolios);
        api.MapGet("{portfolioId:int}", GetPortfolio);
        api.MapPost("/", CreatePortfolio);
        api.MapPut("/deposit", DepositCurrency);
        api.MapPut("{portfolioId:int}/bond", BuyBond);
        api.MapPut("{portfolioId:int}/stock", BuyStock);

        api.MapGet("/produce-operations", ProduceOperations);
        
        return api;
    }

    private static async Task<Ok> ProduceOperations(
        IOperationProducer provider, 
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to,
        CancellationToken token)
    {
        await provider.ProduceOperations(from, to, token);
        return TypedResults.Ok();
    }

    public static async Task<Results<Ok<IEnumerable<PortfolioDTO>>, ProblemHttpResult>> GetPortfolios(
        [AsParameters] PortfolioServices services)
    {
        var results = await services.Mediator.Query(new GetPortfolios());
        return TypedResults.Ok(results);
    }

    public static async Task<Results<Ok<PortfolioDTO>, NotFound>> GetPortfolio(
        int portfolioId,
        [AsParameters] PortfolioServices services)
    {
        var result = await services.Mediator.Query(new GetPortfolio(portfolioId));
        if (result == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<int>, BadRequest<string>>> CreatePortfolio(
        CreatePortfolioRequest request,
        [AsParameters] PortfolioServices services)
    {
        try
        {
            var portfolioId = await services.Mediator.Command(new CreatePortfolio(request.Name));
            return TypedResults.Ok(portfolioId.Id);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Ok> DepositCurrency(
        DepositCurrencyRequest request,
        [AsParameters] PortfolioServices services)
    {
        await services.Mediator.Command(new AddCurrency(request.PortfolioId, request.Money));
        return TypedResults.Ok();
    }

    private static async Task<Ok> BuyStock(
        int portfolioId,
        [FromBody] BuyStockRequest request,
        [AsParameters] PortfolioServices services)
    {
        await services.Mediator.Command(new BuyStock(portfolioId, request.InstrumentId, request.TotalPrice, request.Quantity));
        return TypedResults.Ok();
    }

    private static async Task<Ok> BuyBond(
        int portfolioId,
        [FromBody] BuyBondRequest request,
        [AsParameters] PortfolioServices services)
    {
        await services.Mediator.Command(new BuyBond(portfolioId, request.InstrumentId, request.TotalPrice, request.Quantity));
        return TypedResults.Ok();
    }
}

public record BuyStockRequest(StockId InstrumentId, Money TotalPrice, int Quantity);

public record BuyBondRequest(BondId InstrumentId, Money TotalPrice, int Quantity);

public record CreatePortfolioRequest(string Name);

public record DepositCurrencyRequest(int PortfolioId, Money Money);
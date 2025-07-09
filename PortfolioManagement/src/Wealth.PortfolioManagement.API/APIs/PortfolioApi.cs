using Microsoft.AspNetCore.Http.HttpResults;
using Wealth.PortfolioManagement.Application.Portfolios.Queries;

namespace Wealth.PortfolioManagement.API.APIs;

public static class PortfolioApi
{
    public static RouteGroupBuilder MapPortfolioApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/portfolio");

        api.MapGet("/", GetPortfolios);
        api.MapGet("{portfolioId:guid}", GetPortfolio);

        return api;
    }

    public static async Task<Results<Ok<IEnumerable<PortfolioDTO>>, ProblemHttpResult>> GetPortfolios(
        [AsParameters] PortfolioServices services)
    {
        var results = await services.Mediator.Query(new GetPortfolios());
        return TypedResults.Ok(results);
    }

    public static async Task<Results<Ok<PortfolioDTO>, NotFound>> GetPortfolio(
        Guid portfolioId,
        [AsParameters] PortfolioServices services)
    {
        var result = await services.Mediator.Query(new GetPortfolio(portfolioId));
        if (result == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Application.Wallets.Commands;
using Wealth.WalletManagement.Application.Wallets.Queries;

namespace Wealth.WalletManagement.API.APIs;

public static class WalletApi
{
    public static RouteGroupBuilder MapWalletApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/wallet");

        api.MapGet("/", GetWallets);
        api.MapGet("{walletId:int}", GetWallet);
        api.MapPost("/", CreateWallet);
        api.MapPut("/insert", InsertMoney);
        api.MapPut("/eject", EjectMoney);

        return api;
    }

    private static async Task<Results<Ok<IEnumerable<WalletDTO>>, ProblemHttpResult>> GetWallets(
        [AsParameters] WalletServices services)
    {
        var results = await services.Mediator.Query(new GetWallets());
        return TypedResults.Ok(results);
    }

    private static async Task<Results<Ok<WalletDTO>, NotFound>> GetWallet(
        int walletId,
        [AsParameters] WalletServices services)
    {
        var result = await services.Mediator.Query(new GetWallet(walletId));
        if (result == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<int>, BadRequest<string>>> CreateWallet(
        CreateWalletRequest request,
        [AsParameters] WalletServices services)
    {
        try
        {
            var portfolioId = await services.Mediator.Command(new CreateWallet(request.Name));
            return TypedResults.Ok(portfolioId.Id);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Ok> InsertMoney(
        InsertMoneyRequest request,
        [AsParameters] WalletServices services)
    {
        await services.Mediator.Command(new InsertMoney(request.WalletId, request.Money));
        return TypedResults.Ok();
    }
    
    private static async Task<Ok> EjectMoney(
        EjectMoneyRequest request,
        [AsParameters] WalletServices services)
    {
        await services.Mediator.Command(new EjectMoney(request.WalletId, request.Money));
        return TypedResults.Ok();
    }
}

public record CreateWalletRequest(string Name);
public record InsertMoneyRequest(int WalletId, Money Money);
public record EjectMoneyRequest(int WalletId, Money Money);

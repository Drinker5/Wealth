using Grpc.Core;
using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Application.Deposits.Commands;
using Wealth.DepositManagement.Application.Deposits.Queries;

namespace Wealth.DepositManagement.API.Services;

public class DepositsServiceImpl : DepositsService.DepositsServiceBase
{
    private readonly ILogger<DepositsServiceImpl> logger;
    private readonly ICqrsInvoker cqrsInvoker;

    public DepositsServiceImpl(ILogger<DepositsServiceImpl> logger, ICqrsInvoker cqrsInvoker)
    {
        this.logger = logger;
        this.cqrsInvoker = cqrsInvoker;
    }

    public override async Task<GetDepositsResponse> GetDeposits(GetDepositsRequest request, ServerCallContext context)
    {
        var result = await cqrsInvoker.Query(new GetDeposits());
        return new GetDepositsResponse
        {
            Deposits = { result.Select(i => (DepositProto)i) }
        };
    }

    public override async Task<GetDepositResponse> GetDeposit(GetDepositRequest request, ServerCallContext context)
    {
        var result = await cqrsInvoker.Query(new GetDeposit(request.DepositId));
        if (result == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Deposit not found"));

        return new GetDepositResponse
        {
            Deposit = result,
        };
    }

    public override async Task<CreateDepositResponse> CreateDeposit(CreateDepositRequest request, ServerCallContext context)
    {
        var result = await cqrsInvoker.Command(new CreateDeposit(request.Name, request.Yield, request.Currency));
        return new CreateDepositResponse { DepositId = result };
    }

    public override async Task<EmptyResponse> Invest(InvestRequest request, ServerCallContext context)
    {
        await cqrsInvoker.Command(new DepositInvest(request.DepositId, request.Investment));
        return new EmptyResponse();
    }

    public override async Task<EmptyResponse> Withdraw(WithdrawRequest request, ServerCallContext context)
    {
        await cqrsInvoker.Command(new DepositWithdraw(request.DepositId, request.Withdrawal));
        return new EmptyResponse();
    }
}
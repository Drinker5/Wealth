using MediatR;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public sealed class OperationHandler(
    IPortfolioIdProvider portfolioIdProvider,
    OperationConverter converter,
    ICqrsInvoker mediator,
    IOptions<TBankOperationProviderOptions> options) : IMessageHandler<Tinkoff.InvestApi.V1.Operation>
{
    // TODO BATCH
    public async Task Handle(Tinkoff.InvestApi.V1.Operation operation, CancellationToken token)
    {
        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId, token);
        await foreach (var converted in converter.ConvertOperation(operation, portfolioId).WithCancellation(token))
            await mediator.Command(new AddOperation(converted), token);
    }
}
using Eventso.Subscription;
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Application.Providers;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public sealed class OperationHandler(
    IPortfolioIdProvider portfolioIdProvider,
    OperationConverter converter,
    ICqrsInvoker mediator,
    IOptions<TBankOperationProviderOptions> options) : IMessageHandler<IReadOnlyCollection<Tinkoff.InvestApi.V1.Operation>>
{
    public async Task Handle(IReadOnlyCollection<Tinkoff.InvestApi.V1.Operation> operations, CancellationToken token)
    {
        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId, token);
        foreach (var operation in operations)
            await Handle(operation, portfolioId, token);
    }

    private async Task Handle(Tinkoff.InvestApi.V1.Operation operation, PortfolioId portfolioId, CancellationToken token)
    {
        await foreach (var converted in converter.ConvertOperation(operation, portfolioId).WithCancellation(token))
            await mediator.Command(new AddOperation(converted), token);
    }
}
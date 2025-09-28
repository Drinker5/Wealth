using Wealth.BuildingBlocks.Application;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public sealed class OperationHandler : IMessageHandler<Tinkoff.InvestApi.V1.Operation>
{
    public Task Handle(Tinkoff.InvestApi.V1.Operation message, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
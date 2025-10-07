using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class InputHandler : IOperationHandler
{
    public IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId) => new[]
    {
        new CurrencyOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = new Money(operation.Currency, operation.Payment),
            Type = CurrencyOperationType.Deposit,
            PortfolioId = portfolioId
        }
    }.ToAsyncEnumerable();
}
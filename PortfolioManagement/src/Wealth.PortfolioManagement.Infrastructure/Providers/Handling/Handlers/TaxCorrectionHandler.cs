using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public sealed class TaxCorrectionHandler : IOperationHandler
{
    public IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId,
        CancellationToken token)
    {
        if (instrumentType != InstrumentType.Currency)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        return new[]
        {
            new MoneyOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                Type = MoneyOperationType.TaxCorrection,
                PortfolioId = portfolioId
            }
        }.ToAsyncEnumerable();
    }
}
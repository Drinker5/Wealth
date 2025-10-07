using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BondAmortizationHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        if (instrumentType != InstrumentType.Bond)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        yield return new BondAmortizationOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = operation.Payment.ToMoney(),
            BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
            PortfolioId = portfolioId,
        };
    }
}
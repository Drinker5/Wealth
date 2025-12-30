using System.Runtime.CompilerServices;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BondAmortizationHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        if (instrumentType != InstrumentType.Bond)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        yield return new BondAmortizationOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = operation.Payment.ToMoney(),
            BondId = await instrumentIdProvider.GetBondId(operation.InstrumentUid, token),
            PortfolioId = portfolioId,
        };
    }
}
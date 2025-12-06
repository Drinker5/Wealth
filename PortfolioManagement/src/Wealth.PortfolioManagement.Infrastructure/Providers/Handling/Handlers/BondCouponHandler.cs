using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BondCouponHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        if (instrumentType != Tinkoff.InvestApi.V1.InstrumentType.Bond)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        yield return new BondCouponOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = operation.Payment.ToMoney(),
            PortfolioId = portfolioId,
            BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
        };
    }
}
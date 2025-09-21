using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public class CouponHandler : IOperationHandler
{
    public IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId) => new[]
    {
        new CashOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Money = operation.Payment.ToMoney(),
            PortfolioId = portfolioId,
            Type = CashOperationType.Coupon,
        }
    }.ToAsyncEnumerable();
}
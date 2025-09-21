using System.Collections.Frozen;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Providers;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

internal class OperationConverter(IInstrumentIdProvider instrumentIdProvider)
{
    private static AmortizationHandler _amortizationHandler => new();

    private readonly FrozenDictionary<OperationType, IOperationHandler> _handlers = new Dictionary<OperationType, IOperationHandler>
    {
        { OperationType.BrokerFee, new BrokerFeeHandler(instrumentIdProvider) },
        { OperationType.Buy, new BuyHandler(instrumentIdProvider) },
        { OperationType.BuyCard, new BuyHandler(instrumentIdProvider) },
        { OperationType.Sell, new SellHandler(instrumentIdProvider) },
        { OperationType.Coupon, new CouponHandler() },
        { OperationType.BondRepaymentFull, _amortizationHandler },
        { OperationType.BondRepayment, _amortizationHandler },
        { OperationType.Input, new InputHandler() }
    }.ToFrozenDictionary();

    private static readonly FrozenDictionary<string, InstrumentType> _instrumentTypeMap =
        new Dictionary<string, InstrumentType>
        {
            { "", InstrumentType.Unspecified },
            { "bond", InstrumentType.Bond },
            { "share", InstrumentType.Share },
            { "currency", InstrumentType.Currency },
        }.ToFrozenDictionary();

    public async IAsyncEnumerable<Operation> ConvertOperation(
        Tinkoff.InvestApi.V1.Operation operation,
        PortfolioId portfolioId)
    {
        var instrumentType = _instrumentTypeMap[operation.InstrumentType];
        if (_handlers.TryGetValue(operation.OperationType, out var handler))
        {
            await foreach (var result in handler.Handle(operation, instrumentType, portfolioId))
                yield return result;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Unknown operation type: {operation.OperationType}");
        }
    }
}
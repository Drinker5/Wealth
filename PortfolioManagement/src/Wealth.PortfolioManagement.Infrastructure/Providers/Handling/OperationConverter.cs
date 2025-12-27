using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public sealed class OperationConverter(IServiceProvider sp)
{
    private readonly FrozenDictionary<OperationType, Type> _handlerTypes = new Dictionary<OperationType, Type>
    {
        { OperationType.BrokerFee, typeof(BrokerFeeHandler) },
        { OperationType.Buy, typeof(BuyHandler) },
        { OperationType.BuyCard, typeof(BuyHandler) },
        { OperationType.Sell, typeof(SellHandler) },
        { OperationType.Coupon, typeof(BondCouponHandler) },
        { OperationType.BondRepaymentFull, typeof(BondAmortizationHandler) },
        { OperationType.BondRepayment, typeof(BondAmortizationHandler) },
        { OperationType.Input, typeof(InputHandler) },
        { OperationType.Dividend, typeof(StockDividendHandler) },
        { OperationType.DividendTax, typeof(StockDividendTaxHandler) },
    }.ToFrozenDictionary();

    private readonly Dictionary<OperationType, IOperationHandler> _handlers = new();

    private static readonly FrozenDictionary<string, Tinkoff.InvestApi.V1.InstrumentType> _instrumentTypeMap =
        new Dictionary<string, Tinkoff.InvestApi.V1.InstrumentType>
        {
            { "", Tinkoff.InvestApi.V1.InstrumentType.Unspecified },
            { "bond", Tinkoff.InvestApi.V1.InstrumentType.Bond },
            { "share", Tinkoff.InvestApi.V1.InstrumentType.Share },
            { "currency", Tinkoff.InvestApi.V1.InstrumentType.Currency },
        }.ToFrozenDictionary();

    public async IAsyncEnumerable<Operation> ConvertOperation(
        Tinkoff.InvestApi.V1.Operation operation,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        var instrumentType = _instrumentTypeMap[operation.InstrumentType];

        var handler = GetOrCreateHandler(operation.OperationType);
        await foreach (var result in handler.Handle(operation, instrumentType, portfolioId, token))
            yield return result;
    }
    
    private IOperationHandler GetOrCreateHandler(OperationType operationType)
    {
        if (_handlers.TryGetValue(operationType, out var handler))
            return handler;

        if (_handlerTypes.TryGetValue(operationType, out var handlerType))
        {
            var obj = sp.GetRequiredService(handlerType);
            if (obj is not IOperationHandler operationHandler)
                throw new ApplicationException($"Invalid handler for type {handlerType.Name}");
                    
            _handlers.Add(operationType, operationHandler);
            return operationHandler;
        }

        throw new ArgumentOutOfRangeException($"Unknown operation type: {operationType}");
    }
}
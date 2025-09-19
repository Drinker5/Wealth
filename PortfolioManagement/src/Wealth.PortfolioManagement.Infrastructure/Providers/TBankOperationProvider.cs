using System.Collections.Frozen;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class TBankOperationProvider(
    IPortfolioIdProvider portfolioIdProvider,
    IInstrumentIdProvider instrumentIdProvider,
    IOptions<TBankOperationProviderOptions> options) : IOperationProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    private static readonly FrozenDictionary<string, InstrumentType> InstrumentTypeMap =
        new Dictionary<string, InstrumentType>
        {
            { "bond", InstrumentType.Bond },
            { "share", InstrumentType.Share },
            { "currency", InstrumentType.Currency },
        }.ToFrozenDictionary();

    public async IAsyncEnumerable<Operation> GetOperations(DateTimeOffset from)
    {
        var operations = await client.Operations.GetOperationsAsync(new OperationsRequest
        {
            AccountId = options.Value.AccountId,
            From = Timestamp.FromDateTimeOffset(from.ToUniversalTime()),
            To = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        var portfolioId = await portfolioIdProvider.GetPortfolioIdByAccountId(options.Value.AccountId);
        foreach (var operation in operations.Operations.Where(i => i.State == OperationState.Executed))
        {
            await foreach (var converted in FromProto(operation, portfolioId))
                yield return converted;
        }
    }

    private IAsyncEnumerable<Operation> FromProto(Tinkoff.InvestApi.V1.Operation operation, PortfolioId portfolioId)
    {
        var instrumentType = InstrumentTypeMap[operation.InstrumentType];
        switch (operation.OperationType)
        {
            case OperationType.BrokerFee:
                return BrokerFeeOperation();
            case OperationType.Buy:
                return BuyOperation();
            case OperationType.BuyCard:
                return BuyOperation();
            case OperationType.Sell:
                return SellOperation();
            case OperationType.Coupon:
                return CouponOperation();
            case OperationType.BondRepaymentFull:
            case OperationType.BondRepayment:
                return AmortizationOperation();
            default:
                throw new ArgumentOutOfRangeException($"Unknown operation type: {operation.OperationType}");
        }

        async IAsyncEnumerable<Operation> BrokerFeeOperation()
        {
            yield return instrumentType switch
            {
                InstrumentType.Bond => new BondBrokerFeeOperation
                {
                    Id = operation.Id,
                    Date = operation.Date.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, operation.Payment),
                    BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                },
                InstrumentType.Share => new StockBrokerFeeOperation
                {
                    Id = operation.Id,
                    Date = operation.Date.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, operation.Payment),
                    StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        async IAsyncEnumerable<Operation> BuyOperation()
        {
            foreach (var trade in operation.Trades)
            {
                yield return instrumentType switch
                {
                    InstrumentType.Bond => new BondTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Buy,
                    },
                    InstrumentType.Share => new StockTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Buy,
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        async IAsyncEnumerable<Operation> SellOperation()
        {
            foreach (var trade in operation.Trades)
            {
                yield return instrumentType switch
                {
                    InstrumentType.Bond => new BondTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Sell,
                    },
                    InstrumentType.Share => new StockTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Sell,
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        async IAsyncEnumerable<Operation> CouponOperation()
        {
            yield return new CashOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Money = operation.Payment.ToMoney(),
                PortfolioId = portfolioId,
                Type = CashOperationType.Coupon,
            };
        }
        
        async IAsyncEnumerable<Operation> AmortizationOperation()
        {
            yield return new CashOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Money = operation.Payment.ToMoney(),
                PortfolioId = portfolioId,
                Type = CashOperationType.Amortization,
            };
        }
    }
}

internal static class OperationConverters
{
    public static Money ToMoney(this MoneyValue moneyValue) => new(moneyValue.Currency, moneyValue);
}
using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Application.Providers;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class TBankOperationProvider(
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

    public async Task<IEnumerable<Operation>> GetOperations(DateTimeOffset from)
    {
        var operations = await client.Operations.GetOperationsAsync(new OperationsRequest
        {
            AccountId = options.Value.AccountId,
            From = Timestamp.FromDateTimeOffset(from.ToUniversalTime()),
            To = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        var portfolioId = GetPortfolioIdByAccountId(options.Value.AccountId);
        return operations.Operations
            .Where(i => i.State == OperationState.Executed)
            .SelectMany(i => FromProto(i, portfolioId));
    }

    private static PortfolioId GetPortfolioIdByAccountId(string valueAccountId)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<Operation> FromProto(Tinkoff.InvestApi.V1.Operation operation, PortfolioId portfolioId)
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
            default:
                throw new ArgumentOutOfRangeException();
        }

        IEnumerable<Operation> BrokerFeeOperation()
        {
            yield return instrumentType switch
            {
                InstrumentType.Bond => new BondBrokerFeeOperation
                {
                    Id = operation.Id,
                    Date = operation.Date.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, operation.Payment),
                    BondId = GetBondIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                },
                InstrumentType.Share => new StockBrokerFeeOperation
                {
                    Id = operation.Id,
                    Date = operation.Date.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, operation.Payment),
                    StockId = GetStockIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        IEnumerable<Operation> BuyOperation()
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
                        BondId = GetBondIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Buy,
                    },
                    InstrumentType.Share => new StockTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        StockId = GetStockIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Buy,
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        IEnumerable<Operation> SellOperation()
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
                        BondId = GetBondIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Sell,
                    },
                    InstrumentType.Share => new StockTradeOperation
                    {
                        Id = trade.TradeId,
                        Date = trade.DateTime.ToDateTimeOffset(),
                        Amount = new Money(operation.Currency, trade.Price),
                        StockId = GetStockIdByFigi(operation.Figi),
                        PortfolioId = portfolioId,
                        Quantity = trade.Quantity,
                        Type = TradeOperationType.Sell,
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    private static StockId GetStockIdByFigi(string figi)
    {
        throw new NotImplementedException();
    }

    private static BondId GetBondIdByFigi(string figi)
    {
        throw new NotImplementedException();
    }
}
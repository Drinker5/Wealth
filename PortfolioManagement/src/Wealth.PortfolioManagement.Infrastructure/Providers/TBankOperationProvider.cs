using System.Collections.Frozen;
using System.Security.Cryptography;
using System.Text;
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
        return operations.Operations.Select(i => FromProto(i, portfolioId));
    }

    private static PortfolioId GetPortfolioIdByAccountId(string valueAccountId)
    {
        throw new NotImplementedException();
    }

    private static Operation FromProto(Tinkoff.InvestApi.V1.Operation operation, PortfolioId portfolioId)
    {
        var instrumentType = InstrumentTypeMap[operation.InstrumentType];
        switch (operation.OperationType)
        {
            case OperationType.BrokerFee:
                switch (instrumentType)
                {
                    case InstrumentType.Bond:
                        return new BondBrokerFeeOperation
                        {
                            Id = operation.Id,
                            Date = operation.Date.ToDateTimeOffset(),
                            Amount = new Money(operation.Currency, operation.Payment),
                            BondId = GetBondIdByFigi(operation.Figi),
                            PortfolioId = portfolioId,
                        };
                    case InstrumentType.Share:
                        return new StockBrokerFeeOperation
                        {
                            Id = operation.Id,
                            Date = operation.Date.ToDateTimeOffset(),
                            Amount = new Money(operation.Currency, operation.Payment),
                            StockId = GetStockIdByFigi(operation.Figi),
                            PortfolioId = portfolioId,
                        };
                    case InstrumentType.Currency:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case OperationType.Buy:
                break;
            case OperationType.BuyCard:
                break;
            case OperationType.Sell:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
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

public static class GuidGenerator
{
    public static Guid CreateGuid(this string inputString)
    {
        var inputBytes = Encoding.UTF8.GetBytes(inputString);
        var hashBytes = MD5.HashData(inputBytes);
        return new Guid(hashBytes);
    }
}
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.PortfolioManagement.Application.Providers;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public sealed class TBankOperationProvider(IOptions<TBankOperationProviderOptions> options) : IOperationProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async Task<IEnumerable<Operation>> GetOperations(DateTimeOffset from)
    {
        var operations = await client.Operations.GetOperationsAsync(new OperationsRequest
        {
            AccountId = options.Value.AccountId,
            From = Timestamp.FromDateTimeOffset(from.ToUniversalTime()),
            To = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        return operations.Operations.Select(FromProto);
    }

    private static Operation FromProto(Tinkoff.InvestApi.V1.Operation operation)
    {
        switch (operation.OperationType)
        {
            case OperationType.Unspecified:
                break;
            case OperationType.Input:
                break;
            case OperationType.BondTax:
                break;
            case OperationType.OutputSecurities:
                break;
            case OperationType.Overnight:
                break;
            case OperationType.Tax:
                break;
            case OperationType.BondRepaymentFull:
                break;
            case OperationType.SellCard:
                break;
            case OperationType.DividendTax:
                break;
            case OperationType.Output:
                break;
            case OperationType.BondRepayment:
                break;
            case OperationType.TaxCorrection:
                break;
            case OperationType.ServiceFee:
                break;
            case OperationType.BenefitTax:
                break;
            case OperationType.MarginFee:
                break;
            case OperationType.Buy:
                break;
            case OperationType.BuyCard:
                break;
            case OperationType.InputSecurities:
                break;
            case OperationType.SellMargin:
                break;
            case OperationType.BrokerFee:
                break;
            case OperationType.BuyMargin:
                break;
            case OperationType.Dividend:
                break;
            case OperationType.Sell:
                break;
            case OperationType.Coupon:
                break;
            case OperationType.SuccessFee:
                break;
            case OperationType.DividendTransfer:
                break;
            case OperationType.AccruingVarmargin:
                break;
            case OperationType.WritingOffVarmargin:
                break;
            case OperationType.DeliveryBuy:
                break;
            case OperationType.DeliverySell:
                break;
            case OperationType.TrackMfee:
                break;
            case OperationType.TrackPfee:
                break;
            case OperationType.TaxProgressive:
                break;
            case OperationType.BondTaxProgressive:
                break;
            case OperationType.DividendTaxProgressive:
                break;
            case OperationType.BenefitTaxProgressive:
                break;
            case OperationType.TaxCorrectionProgressive:
                break;
            case OperationType.TaxRepoProgressive:
                break;
            case OperationType.TaxRepo:
                break;
            case OperationType.TaxRepoHold:
                break;
            case OperationType.TaxRepoRefund:
                break;
            case OperationType.TaxRepoHoldProgressive:
                break;
            case OperationType.TaxRepoRefundProgressive:
                break;
            case OperationType.DivExt:
                break;
            case OperationType.TaxCorrectionCoupon:
                break;
            case OperationType.CashFee:
                break;
            case OperationType.OutFee:
                break;
            case OperationType.OutStampDuty:
                break;
            case OperationType.OutputSwift:
                break;
            case OperationType.InputSwift:
                break;
            case OperationType.OutputAcquiring:
                break;
            case OperationType.InputAcquiring:
                break;
            case OperationType.OutputPenalty:
                break;
            case OperationType.AdviceFee:
                break;
            case OperationType.TransIisBs:
                break;
            case OperationType.TransBsBs:
                break;
            case OperationType.OutMulti:
                break;
            case OperationType.InpMulti:
                break;
            case OperationType.OverPlacement:
                break;
            case OperationType.OverCom:
                break;
            case OperationType.OverIncome:
                break;
            case OperationType.OptionExpiration:
                break;
            case OperationType.FutureExpiration:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return null;
    }
}
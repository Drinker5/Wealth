using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class TBankInstrumentsProvider(
    IOptions<TBankInstrumentsProviderOptions> options,
    ILogger<TBankInstrumentsProvider> logger) : IInstrumentsProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);

    public async ValueTask<CreateStockCommand> StockProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var stock = await client.Instruments.ShareByAsync(new InstrumentRequest
            {
                IdType = Tinkoff.InvestApi.V1.InstrumentIdType.Uid,
                Id = instrumentUId.ToString()
            }, cancellationToken: token);


            return new CreateStockCommand
            {
                Name = stock.Instrument.Name,
                InstrumentUId = stock.Instrument.Uid,
                Isin = stock.Instrument.Isin,
                Figi = stock.Instrument.Figi,
                LotSize = stock.Instrument.Lot,
                Ticker = stock.Instrument.Ticker,
                Currency = CurrencyCodeParser.Parse(stock.Instrument.Currency)
            };
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            logger.LogError(e, "Stock not found {InstrumentId}", instrumentUId);
            throw;
        }
    }

    public async ValueTask<CreateBondCommand> BondProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var bond = await client.Instruments.BondByAsync(new InstrumentRequest
            {
                IdType = Tinkoff.InvestApi.V1.InstrumentIdType.Uid,
                Id = instrumentUId.ToString()
            }, cancellationToken: token);

            return new CreateBondCommand
            {
                Name = bond.Instrument.Name,
                InstrumentUId = bond.Instrument.Uid,
                Isin = bond.Instrument.Isin,
                Figi = bond.Instrument.Figi,
                Currency = CurrencyCodeParser.Parse(bond.Instrument.Currency),
                Nominal = new Money(CurrencyCodeParser.Parse(bond.Instrument.Nominal.Currency), bond.Instrument.Nominal)
            };
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            logger.LogError(e, "Bond not found {InstrumentId}", instrumentUId);
            throw;
        }
    }

    public async ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        try
        {
            var currency = await client.Instruments.CurrencyByAsync(new InstrumentRequest
            {
                IdType = Tinkoff.InvestApi.V1.InstrumentIdType.Uid,
                Id = instrumentUId.ToString()
            }, cancellationToken: token);

            return new CreateCurrencyCommand
            {
                Name = currency.Instrument.Name,
                InstrumentUId = currency.Instrument.Uid,
                Figi = currency.Instrument.Figi,
                Currency = CurrencyCodeParser.Parse(currency.Instrument.Currency_)
            };
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            logger.LogError(e, "Currency not found {InstrumentId}", instrumentUId);
            throw;
        }
    }
}
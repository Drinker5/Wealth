using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class InstrumentsProvider(IOptions<TBankInstrumentsProviderOptions> options) : IInstrumentsProvider
{
    private readonly InvestApiClient client = InvestApiClientFactory.Create(options.Value.Token);
    
    public async ValueTask<CreateStockCommand> StockProvide(InstrumentId instrumentId, CancellationToken token)
    {
        var stock = await client.Instruments.ShareByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Uid,
            Id = instrumentId.ToString()
        }, cancellationToken: token);

        return new CreateStockCommand
        {
            Name = stock.Instrument.Name,
            InstrumentId = stock.Instrument.Uid,
            Isin = stock.Instrument.Isin,
            Figi = stock.Instrument.Figi,
            LotSize = stock.Instrument.Lot,
            Ticker = stock.Instrument.Ticker
        };
    }

    public async ValueTask<CreateBondCommand> BondProvide(InstrumentId instrumentId, CancellationToken token)
    {
        var bond = await client.Instruments.BondByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Uid,
            Id = instrumentId.ToString()
        }, cancellationToken: token);

        return new CreateBondCommand
        {
            Name = bond.Instrument.Name,
            InstrumentId = bond.Instrument.Uid,
            Isin = bond.Instrument.Isin,
            Figi = bond.Instrument.Figi,
        };
    }

    public async ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentId instrumentId, CancellationToken token)
    {
        var currency = await client.Instruments.CurrencyByAsync(new InstrumentRequest
        {
            IdType = InstrumentIdType.Uid,
            Id = instrumentId.ToString()
        }, cancellationToken: token);

        return new CreateCurrencyCommand
        {
            Name = currency.Instrument.Name,
            InstrumentId = currency.Instrument.Uid,
            Figi = currency.Instrument.Figi
        };
    }
}
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;
using Instrument = Wealth.InstrumentManagement.Application.Instruments.Models.Instrument;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public class InstrumentsProviderDecorator(IInstrumentsProvider instrumentsProvider) : IInstrumentsProvider
{
    private Dictionary<InstrumentId, CreateBondCommand>? Bonds = null;
    private Dictionary<InstrumentId, CreateStockCommand>? Stocks = null;
    private Dictionary<InstrumentId, CreateCurrencyCommand>? Currencies = null;

    public async ValueTask<CreateStockCommand> StockProvide(InstrumentId instrumentId, CancellationToken token)
    {
        Stocks ??= new Dictionary<InstrumentId, CreateStockCommand>();
        if (!Stocks.ContainsKey(instrumentId))
            Stocks[instrumentId] = await instrumentsProvider.StockProvide(instrumentId, token);

        return Stocks[instrumentId];
    }

    public async ValueTask<CreateBondCommand> BondProvide(InstrumentId instrumentId, CancellationToken token)
    {
        Bonds ??= new Dictionary<InstrumentId, CreateBondCommand>();
        if (!Bonds.ContainsKey(instrumentId))
            Bonds[instrumentId] = await instrumentsProvider.BondProvide(instrumentId, token);

        return Bonds[instrumentId];
    }

    public async ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentId instrumentId, CancellationToken token)
    {
        Currencies ??= new Dictionary<InstrumentId, CreateCurrencyCommand>();
        if (!Currencies.ContainsKey(instrumentId))
            Currencies[instrumentId] = await instrumentsProvider.CurrencyProvide(instrumentId, token);

        return Currencies[instrumentId];
    }
}
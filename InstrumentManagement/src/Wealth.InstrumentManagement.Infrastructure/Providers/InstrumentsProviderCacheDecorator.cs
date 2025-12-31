using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class InstrumentsProviderDecorator(IInstrumentsProvider instrumentsProvider) : IInstrumentsProvider
{
    private Dictionary<InstrumentUId, CreateBondCommand>? Bonds = null;
    private Dictionary<InstrumentUId, CreateStockCommand>? Stocks = null;
    private Dictionary<InstrumentUId, CreateCurrencyCommand>? Currencies = null;

    public async ValueTask<CreateStockCommand> StockProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Stocks ??= new Dictionary<InstrumentUId, CreateStockCommand>();
        if (!Stocks.ContainsKey(instrumentUId))
            Stocks[instrumentUId] = await instrumentsProvider.StockProvide(instrumentUId, token);

        return Stocks[instrumentUId];
    }

    public async ValueTask<CreateBondCommand> BondProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Bonds ??= new Dictionary<InstrumentUId, CreateBondCommand>();
        if (!Bonds.ContainsKey(instrumentUId))
            Bonds[instrumentUId] = await instrumentsProvider.BondProvide(instrumentUId, token);

        return Bonds[instrumentUId];
    }

    public async ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Currencies ??= new Dictionary<InstrumentUId, CreateCurrencyCommand>();
        if (!Currencies.ContainsKey(instrumentUId))
            Currencies[instrumentUId] = await instrumentsProvider.CurrencyProvide(instrumentUId, token);

        return Currencies[instrumentUId];
    }
}
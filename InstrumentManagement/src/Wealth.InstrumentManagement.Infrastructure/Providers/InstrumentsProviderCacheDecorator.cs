using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.Infrastructure.Providers;

public sealed class InstrumentsProviderDecorator(IInstrumentsProvider instrumentsProvider) : IInstrumentsProvider
{
    private Dictionary<InstrumentUId, CreateBondCommand>? Bonds = null;
    private Dictionary<InstrumentUId, CreateStockCommand>? Stocks = null;
    private Dictionary<InstrumentUId, CreateCurrencyCommand>? Currencies = null;

    public async ValueTask<CreateStockCommand?> StockProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Stocks ??= new Dictionary<InstrumentUId, CreateStockCommand>();
        if (!Stocks.ContainsKey(instrumentUId))
        {
            var command = await instrumentsProvider.StockProvide(instrumentUId, token);
            if (command == null)
                return null;

            Stocks[instrumentUId] = command.Value;
        }

        return Stocks[instrumentUId];
    }

    public async ValueTask<CreateBondCommand?> BondProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Bonds ??= new Dictionary<InstrumentUId, CreateBondCommand>();
        if (!Bonds.ContainsKey(instrumentUId))
        {
            var command = await instrumentsProvider.BondProvide(instrumentUId, token);
            if (command == null)
                return null;

            Bonds[instrumentUId] = command.Value;
        }

        return Bonds[instrumentUId];
    }

    public async ValueTask<CreateCurrencyCommand?> CurrencyProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        Currencies ??= new Dictionary<InstrumentUId, CreateCurrencyCommand>();
        if (!Currencies.ContainsKey(instrumentUId))
        {
            var command = await instrumentsProvider.CurrencyProvide(instrumentUId, token);
            if (command == null)
                return null;

            Currencies[instrumentUId] = command.Value;
        }

        return Currencies[instrumentUId];
    }
}
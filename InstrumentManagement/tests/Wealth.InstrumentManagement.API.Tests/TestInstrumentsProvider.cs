using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class TestInstrumentsProvider : IInstrumentsProvider
{
    public static Dictionary<InstrumentUId, CreateStockCommand> Stocks = new();
    public static Dictionary<InstrumentUId, CreateBondCommand> Bonds = new();
    public static Dictionary<InstrumentUId, CreateCurrencyCommand> Currencies = new();

    public ValueTask<CreateStockCommand> StockProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        return ValueTask.FromResult(Stocks[instrumentUId]);
    }

    public ValueTask<CreateBondCommand> BondProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        return ValueTask.FromResult(Bonds[instrumentUId]);
    }

    public ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentUId instrumentUId, CancellationToken token)
    {
        return ValueTask.FromResult(Currencies[instrumentUId]);
    }
}
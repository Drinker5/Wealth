using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Providers;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class TestInstrumentsProvider : IInstrumentsProvider
{
    public static Dictionary<InstrumentId, CreateStockCommand> Stocks = new();
    public static Dictionary<InstrumentId, CreateBondCommand> Bonds = new();
    public static Dictionary<InstrumentId, CreateCurrencyCommand> Currencies = new();

    public ValueTask<CreateStockCommand> StockProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return ValueTask.FromResult(Stocks[instrumentId]);
    }

    public ValueTask<CreateBondCommand> BondProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return ValueTask.FromResult(Bonds[instrumentId]);
    }

    public ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentId instrumentId, CancellationToken token)
    {
        return ValueTask.FromResult(Currencies[instrumentId]);
    }
}
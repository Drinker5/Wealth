using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;

namespace Wealth.InstrumentManagement.Application.Providers;

public interface IInstrumentsProvider
{
    ValueTask<CreateStockCommand> StockProvide(InstrumentId instrumentId, CancellationToken token);

    ValueTask<CreateBondCommand> BondProvide(InstrumentId instrumentId, CancellationToken token);

    ValueTask<CreateCurrencyCommand> CurrencyProvide(InstrumentId instrumentId, CancellationToken token);
}

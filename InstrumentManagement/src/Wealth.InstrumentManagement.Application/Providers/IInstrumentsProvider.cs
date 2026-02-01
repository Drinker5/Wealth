using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;

namespace Wealth.InstrumentManagement.Application.Providers;

public interface IInstrumentsProvider
{
    ValueTask<CreateStockCommand?> StockProvide(InstrumentUId instrumentUId, CancellationToken token);

    ValueTask<CreateBondCommand?> BondProvide(InstrumentUId instrumentUId, CancellationToken token);

    ValueTask<CreateCurrencyCommand?> CurrencyProvide(InstrumentUId instrumentUId, CancellationToken token);
}
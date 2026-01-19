using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateStockCommand(
    Ticker Ticker,
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentUId InstrumentUId,
    LotSize LotSize,
    CurrencyCode Currency) : ICommand<StockId>;

public class CreateStockCommandHandler(IStocksRepository repository) : ICommandHandler<CreateStockCommand, StockId>
{
    public Task<StockId> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateStock(request, cancellationToken);
    }
}
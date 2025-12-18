using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateStockCommandHandler(IStocksRepository repository)
    : ICommandHandler<CreateStockCommand, StockId>
{
    public Task<StockId> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateStock(
            request.Ticker,
            request.Name,
            request.Isin,
            request.Figi,
            request.LotSize,
            cancellationToken);
    }
}
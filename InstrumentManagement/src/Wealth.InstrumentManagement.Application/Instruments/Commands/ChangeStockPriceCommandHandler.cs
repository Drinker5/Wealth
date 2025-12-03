using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeStockPriceCommandHandler(IStocksRepository repository) : ICommandHandler<ChangeStockPriceCommand>
{
    public async Task Handle(ChangeStockPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.StockId, request.Price);
    }
}
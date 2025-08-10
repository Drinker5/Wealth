using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeStockPriceCommandHandler(IStocksRepository repository, ICurrencyService currencyService) : ICommandHandler<ChangeStockPriceCommand>
{
    public async Task Handle(ChangeStockPriceCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Price.CurrencyId))
            return;
        
        await repository.ChangePrice(request.StockId, request.Price);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCurrencyPriceCommandHandler(ICurrenciesRepository repository, ICurrencyService currencyService) : ICommandHandler<ChangeCurrencyPriceCommand>
{
    public async Task Handle(ChangeCurrencyPriceCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Price.Currency))
            return;
        
        await repository.ChangePrice(request.CurrencyId, request.Price);
    }
}
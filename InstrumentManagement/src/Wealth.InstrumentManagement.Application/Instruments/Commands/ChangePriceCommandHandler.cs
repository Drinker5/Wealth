using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangePriceCommandHandler(IInstrumentsRepository repository, ICurrencyService currencyService) : ICommandHandler<ChangePriceCommand>
{
    public async Task Handle(ChangePriceCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Price.CurrencyId))
            return;
        
        await repository.ChangePrice(request.Id, request.Price);
    }
}
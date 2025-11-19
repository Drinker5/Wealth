using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Services;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeBondPriceCommandHandler(IBondsRepository repository, ICurrencyService currencyService) : ICommandHandler<ChangeBondPriceCommand>
{
    public async Task Handle(ChangeBondPriceCommand request, CancellationToken cancellationToken)
    {
        if (!await currencyService.IsCurrencyExists(request.Price.Currency))
            return;
        
        await repository.ChangePrice(request.BondId, request.Price);
    }
}
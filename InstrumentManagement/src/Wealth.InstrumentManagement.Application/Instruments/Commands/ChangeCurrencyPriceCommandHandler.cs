using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeCurrencyPriceCommandHandler(ICurrenciesRepository repository) : ICommandHandler<ChangeCurrencyPriceCommand>
{
    public async Task Handle(ChangeCurrencyPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.CurrencyId, request.Price);
    }
}
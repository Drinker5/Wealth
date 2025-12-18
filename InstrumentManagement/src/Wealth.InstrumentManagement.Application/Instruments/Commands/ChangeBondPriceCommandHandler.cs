using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class ChangeBondPriceCommandHandler(IBondsRepository repository) : ICommandHandler<ChangeBondPriceCommand>
{
    public async Task Handle(ChangeBondPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.BondId, request.Price);
    }
}
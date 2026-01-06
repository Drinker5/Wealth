using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct ChangeBondPriceCommand(BondId BondId, Money Price) : ICommand;

public class ChangeBondPriceCommandHandler(IBondsRepository repository) : ICommandHandler<ChangeBondPriceCommand>
{
    public async Task Handle(ChangeBondPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.BondId, request.Price);
    }
}
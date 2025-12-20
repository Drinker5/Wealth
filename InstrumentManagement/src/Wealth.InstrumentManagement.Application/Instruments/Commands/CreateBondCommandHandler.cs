using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateBondCommandHandler(IBondsRepository repository) : ICommandHandler<CreateBondCommand, BondId>
{
    public Task<BondId> Handle(CreateBondCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateBond(request, cancellationToken);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public class CreateBondCommandHandler : ICommandHandler<CreateBondCommand, InstrumentId>
{
    private readonly IBondsRepository repository;

    public CreateBondCommandHandler(IBondsRepository repository)
    {
        this.repository = repository;
    }

    public Task<InstrumentId> Handle(CreateBondCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateBond(request.Name, request.ISIN);
    }
}
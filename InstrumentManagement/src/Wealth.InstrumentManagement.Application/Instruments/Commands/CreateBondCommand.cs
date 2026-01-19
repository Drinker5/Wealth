using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct CreateBondCommand(
    string Name,
    ISIN Isin,
    FIGI Figi,
    InstrumentUId InstrumentUId,
    CurrencyCode Currency) : ICommand<BondId>;

public class CreateBondCommandHandler(IBondsRepository repository) : ICommandHandler<CreateBondCommand, BondId>
{
    public Task<BondId> Handle(CreateBondCommand request, CancellationToken cancellationToken)
    {
        return repository.CreateBond(request, cancellationToken);
    }
}
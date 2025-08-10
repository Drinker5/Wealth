using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetBondQueryHandler(IBondsRepository repository) : IQueryHandler<GetBond, Bond?>
{
    public async Task<Bond?> Handle(GetBond request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetBond(request.Id);
        return instrument;
    }
}
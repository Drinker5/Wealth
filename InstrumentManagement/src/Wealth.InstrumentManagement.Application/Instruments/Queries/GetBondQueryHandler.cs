using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetBondQueryHandler(IBondsRepository repository) : 
    IQueryHandler<GetBond, Bond?>,
    IQueryHandler<GetBondByIsin, Bond?>,
    IQueryHandler<GetBondByFigi, Bond?>,
    IQueryHandler<GetBondsQuery, IReadOnlyCollection<Bond>>
{
    public async Task<Bond?> Handle(GetBond request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetBond(request.Id);
        return instrument;
    }

    public async Task<Bond?> Handle(GetBondByIsin request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetBond(request.Isin);
        return instrument;
    }

    public async Task<Bond?> Handle(GetBondByFigi request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetBond(request.Figi);
        return instrument;
    }

    public async Task<IReadOnlyCollection<Bond>> Handle(GetBondsQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetBonds();
        return instruments;
    }
}
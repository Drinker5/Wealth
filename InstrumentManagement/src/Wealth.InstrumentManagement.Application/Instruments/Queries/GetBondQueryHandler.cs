using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetBondQueryHandler(IBondsRepository repository) :
    IQueryHandler<GetBond, Bond?>,
    IQueryHandler<GetBondByIsin, Bond?>,
    IQueryHandler<GetBondByFigi, Bond?>,
    IQueryHandler<GetBondByInstrumentId, Bond?>,
    IQueryHandler<GetBondsQuery, IReadOnlyCollection<Bond>>
{
    public Task<Bond?> Handle(GetBond request, CancellationToken cancellationToken)
    {
        return repository.GetBond(request.Id);
    }

    public Task<Bond?> Handle(GetBondByIsin request, CancellationToken cancellationToken)
    {
        return repository.GetBond(request.Isin);
    }

    public Task<Bond?> Handle(GetBondByFigi request, CancellationToken cancellationToken)
    {
        return repository.GetBond(request.Figi);
    }

    public Task<Bond?> Handle(GetBondByInstrumentId request, CancellationToken cancellationToken)
    {
        return repository.GetBond(request.UId);
    }

    public async Task<IReadOnlyCollection<Bond>> Handle(GetBondsQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetBonds();
        return instruments;
    }
}
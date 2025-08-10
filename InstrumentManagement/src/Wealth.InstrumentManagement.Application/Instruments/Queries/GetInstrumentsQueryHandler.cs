using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetBondsQueryHandler : IQueryHandler<GetBondsQuery, IReadOnlyCollection<Bond>>
{
    private readonly IBondsRepository repository;

    public GetBondsQueryHandler(IBondsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IReadOnlyCollection<Bond>> Handle(GetBondsQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetBonds();
        return instruments;
    }
}
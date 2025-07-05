using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetInstrumentsQueryHandler : IQueryHandler<GetInstrumentsQuery, IEnumerable<InstrumentDTO>>
{
    private readonly IInstrumentsRepository repository;

    public GetInstrumentsQueryHandler(IInstrumentsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<InstrumentDTO>> Handle(GetInstrumentsQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetInstruments();
        return instruments.Select(InstrumentDTO.From).ToArray();
    }
}
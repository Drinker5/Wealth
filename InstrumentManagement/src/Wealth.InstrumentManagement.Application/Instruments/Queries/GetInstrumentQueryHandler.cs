using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetInstrumentQueryHandler : IQueryHandler<GetInstrumentQuery, InstrumentDTO?>
{
    private readonly IInstrumentsRepository repository;

    public GetInstrumentQueryHandler(IInstrumentsRepository repository)
    {
        this.repository = repository;
    }

    public async Task<InstrumentDTO?> Handle(GetInstrumentQuery request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetInstrument(request.Id);
        if (instrument == null)
            return null;

        return InstrumentDTO.From(instrument);
    }
}
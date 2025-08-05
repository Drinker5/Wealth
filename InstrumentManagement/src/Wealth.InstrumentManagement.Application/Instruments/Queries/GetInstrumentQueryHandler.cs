using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetInstrumentQueryHandler(IInstrumentsRepository repository) : IQueryHandler<GetInstrument, Instrument?>
{
    public async Task<Instrument?> Handle(GetInstrument request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetInstrument(request.Id);
        if (instrument == null)
            return null;

        return instrument;
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetInstrumentByIsinQueryHandler(IInstrumentsRepository repository) : IQueryHandler<GetInstrumentByIsin, Instrument?>
{
    public Task<Instrument?> Handle(GetInstrumentByIsin request, CancellationToken cancellationToken)
    {
        return repository.GetInstrument(request.Isin);
    }
}
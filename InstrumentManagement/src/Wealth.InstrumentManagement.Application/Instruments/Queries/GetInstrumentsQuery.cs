using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record struct GetInstrumentsQuery(IReadOnlyCollection<string> Isins) : IQuery<IReadOnlyCollection<Instrument>>;

public sealed class GetInstrumentsQueryHandler(IInstrumentsRepository repository) :
    IQueryHandler<GetInstrumentsQuery, IReadOnlyCollection<Instrument>>
{
    public Task<IReadOnlyCollection<Instrument>> Handle(GetInstrumentsQuery request, CancellationToken cancellationToken)
    {
        return repository.GetInstruments(request.Isins, cancellationToken);
    }
}
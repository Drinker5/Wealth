using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetCurrencyQueryHandler(ICurrenciesRepository repository) : 
    IQueryHandler<GetCurrency, Currency?>,
    IQueryHandler<GetCurrencyByFigi, Currency?>,
    IQueryHandler<GetCurrenciesQuery, IReadOnlyCollection<Currency>>
{
    public async Task<Currency?> Handle(GetCurrency request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetCurrency(request.Id);
        return instrument;
    }

    public async Task<Currency?> Handle(GetCurrencyByFigi request, CancellationToken cancellationToken)
    {
        var instrument = await repository.GetCurrency(request.Figi);
        return instrument;
    }

    public async Task<IReadOnlyCollection<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetCurrencies();
        return instruments;
    }
}
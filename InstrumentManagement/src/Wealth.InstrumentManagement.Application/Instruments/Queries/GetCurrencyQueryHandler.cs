using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public class GetCurrencyQueryHandler(ICurrenciesRepository repository) : 
    IQueryHandler<GetCurrency, Currency?>,
    IQueryHandler<GetCurrencyByFigi, Currency?>,
    IQueryHandler<GetCurrencyByInstrumentId, Currency?>,
    IQueryHandler<GetCurrenciesQuery, IReadOnlyCollection<Currency>>
{
    public  Task<Currency?> Handle(GetCurrency request, CancellationToken cancellationToken)
    {
        return repository.GetCurrency(request.Id);
    }

    public  Task<Currency?> Handle(GetCurrencyByFigi request, CancellationToken cancellationToken)
    {
        return repository.GetCurrency(request.Figi);
    }

    public Task<Currency?> Handle(GetCurrencyByInstrumentId request, CancellationToken cancellationToken)
    {
        return repository.GetCurrency(request.UId);
    }

    public async Task<IReadOnlyCollection<Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var instruments = await repository.GetCurrencies();
        return instruments;
    }
}
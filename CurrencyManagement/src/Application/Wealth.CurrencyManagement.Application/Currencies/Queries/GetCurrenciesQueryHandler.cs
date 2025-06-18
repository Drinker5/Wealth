using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IEnumerable<CurrencyDTO>>
{
    private readonly ICurrencyRepository currencyRepository;

    public GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository)
    {
        this.currencyRepository = currencyRepository;
    }

    public async Task<IEnumerable<CurrencyDTO>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var currencies = await currencyRepository.GetCurrencies();
        return currencies.Select(CurrencyDTO.From).ToArray();
    }
}
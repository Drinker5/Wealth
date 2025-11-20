using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public class GetCurrenciesQueryHandler : IQueryHandler<GetCurrenciesQuery, IEnumerable<CurrencyDTO>>
{
    public Task<IEnumerable<CurrencyDTO>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<CurrencyDTO>>(Enum.GetValues<CurrencyCode>()
            .Select(CurrencyDTO.From)
            .ToArray());
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public class GetCurrencyQueryHandler(ICurrencyRepository repository) : IQueryHandler<GetCurrencyQuery, CurrencyDTO?>
{
    public async Task<CurrencyDTO?> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetCurrency(request.Id);
        if (result is null)
            return null;

        return CurrencyDTO.From(result);
    }
}
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public record GetExchangeRatesQuery(
    CurrencyCode FromId,
    CurrencyCode ToId,
    int? Page = 1,
    int? PageSize = 50) : IQuery<PaginatedResult<ExchangeRateDTO>>;

public class GetExchangeRatesQueryHandler : IQueryHandler<GetExchangeRatesQuery, PaginatedResult<ExchangeRateDTO>>
{
    private readonly IExchangeRateRepository exchangeRateRepository;

    public GetExchangeRatesQueryHandler(IExchangeRateRepository exchangeRateRepository)
    {
        this.exchangeRateRepository = exchangeRateRepository;
    }

    public async Task<PaginatedResult<ExchangeRateDTO>> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        if (request.Page is < 1)
            throw new ArgumentException("Page must be greater than or equal to 1.", nameof(request.Page));
        if (request.PageSize is < 1)
            throw new ArgumentException("PageSize must be greater than or equal to 1.", nameof(request.PageSize));

        var result = await exchangeRateRepository.GetExchangeRates(request.FromId, request.ToId, new PageRequest(request.Page ?? 1, request.PageSize ?? 50));
        return new PaginatedResult<ExchangeRateDTO>(result.Items.Select(ExchangeRateDTO.From).ToArray().AsReadOnly(), result.TotalCount);
    }
}
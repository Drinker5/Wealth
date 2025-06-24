using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public class ExchangeQueryHandler : IQueryHandler<ExchangeQuery, Money?>
{
    private readonly IExchangeRateRepository exchangeRateRepository;

    public ExchangeQueryHandler(IExchangeRateRepository exchangeRateRepository)
    {
        this.exchangeRateRepository = exchangeRateRepository;
    }

    public async Task<Money?> Handle(ExchangeQuery request, CancellationToken cancellationToken)
    {
        var exchangeRate = await exchangeRateRepository.GetExchangeRate(request.Money.CurrencyId, request.TargetCurrencyId, request.Date);
        return exchangeRate?.Convert(request.Money);
    }
}
using Wealth.CurrencyManagement.Domain.Entities;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure;

public class InMemoryExchangeRateRepository : IExchangeRateRepository
{
    public Task<ExchangeRate> GetExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, DateTime validOnDate)
    {
        throw new NotImplementedException();
    }

    public Task<ExchangeRate> CreateExchangeRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, decimal rate, DateTime validOnDate)
    {
        throw new NotImplementedException();
    }
}
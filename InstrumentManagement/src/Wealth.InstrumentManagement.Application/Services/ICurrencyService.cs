using Wealth.InstrumentManagement.Domain;

namespace Wealth.InstrumentManagement.Application.Services;

public interface ICurrencyService
{
    Task<bool> IsCurrencyExists(CurrencyId currency);
}
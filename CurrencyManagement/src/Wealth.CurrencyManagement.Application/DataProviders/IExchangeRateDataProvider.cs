using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.DataProviders;

public interface IExchangeRateDataProvider
{
    Task<decimal> GetRate(CurrencyId baseCurrencyId, CurrencyId targetCurrencyId, DateOnly? validOnDate);
}
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.DataProviders;

public interface IExchangeRateDataProvider
{
    Task<decimal> GetRate(CurrencyCode baseCurrency, CurrencyCode targetCurrency, DateOnly? validOnDate);
}
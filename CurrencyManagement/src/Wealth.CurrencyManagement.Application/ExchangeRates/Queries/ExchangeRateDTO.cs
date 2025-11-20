using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public record ExchangeRateDTO(
    CurrencyCode FromId,
    CurrencyCode ToId,
    DateOnly ValidOnDate,
    decimal Rate)
{
    public static ExchangeRateDTO From(ExchangeRate exchangeRate)
    {
        return new ExchangeRateDTO(
            exchangeRate.BaseCurrency,
            exchangeRate.TargetCurrency,
            exchangeRate.ValidOnDate,
            exchangeRate.Rate);
    }
}
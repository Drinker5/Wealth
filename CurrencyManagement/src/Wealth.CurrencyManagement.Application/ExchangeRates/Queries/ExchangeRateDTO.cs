using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Queries;

public record ExchangeRateDTO(
    CurrencyId FromId,
    CurrencyId ToId,
    DateOnly ValidOnDate,
    decimal Rate)
{
    public static ExchangeRateDTO From(ExchangeRate exchangeRate)
    {
        return new ExchangeRateDTO(
            exchangeRate.BaseCurrencyId,
            exchangeRate.TargetCurrencyId,
            exchangeRate.ValidOnDate,
            exchangeRate.Rate);
    }
}
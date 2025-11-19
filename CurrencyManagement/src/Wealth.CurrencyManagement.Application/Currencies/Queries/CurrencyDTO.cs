using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public record CurrencyDTO(
    byte CurrencyCodeId,
    string Name)
{
    public static CurrencyDTO From(CurrencyCode currency)
    {
        return new CurrencyDTO((byte)currency, currency.ToString());
    }
}
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

public record CurrencyDTO(
    CurrencyId CurrencyId,
    string Name,
    string Symbol)
{
    public static CurrencyDTO From(Currency currency)
    {
        return new CurrencyDTO(currency.Id, currency.Name, currency.Symbol);
    }
}
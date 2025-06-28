using Wealth.InstrumentManagement.Domain;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record MoneyDTO(string code, decimal value)
{
    public static MoneyDTO From(Money money)
    {
        return new MoneyDTO(money.CurrencyId, money.Amount);
    }
}
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class MoneyProto
{
    public MoneyProto(CurrencyId currencyId, decimal amount)
    {
        CurrencyId = currencyId;
        Amount = amount;
    }

    public static implicit operator Money(MoneyProto grpcMoney)
    {
        return new Money(grpcMoney.CurrencyId, grpcMoney.Amount);
    }

    public static implicit operator MoneyProto(Money money)
    {
        return new MoneyProto(money.CurrencyId, money.Value);
    }
}
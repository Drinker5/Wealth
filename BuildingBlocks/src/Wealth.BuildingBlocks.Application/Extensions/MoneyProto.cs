using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class MoneyProto
{
    public MoneyProto(decimal amount, CurrencyId currencyId)
    {
        Amount = amount;
        CurrencyId = currencyId;
    }

    public static implicit operator Money(MoneyProto grpcMoney)
    {
        return new Money(grpcMoney.CurrencyId.Code, grpcMoney.Amount);
    }

    public static implicit operator MoneyProto(Money money)
    {
        return new MoneyProto(money.Amount, money.CurrencyId);
    }
}
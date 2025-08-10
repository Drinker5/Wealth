using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class MoneyProto
{
    public MoneyProto(decimal value, CurrencyId currencyId)
    {
        Price = value;
        CurrencyId = currencyId;
    }

    public static implicit operator Money(MoneyProto grpcMoney)
    {
        return new Money(grpcMoney.CurrencyId.Code, grpcMoney.Price);
    }

    public static implicit operator MoneyProto(Money value)
    {
        return new MoneyProto(value.Amount, value.CurrencyId);
    }
}
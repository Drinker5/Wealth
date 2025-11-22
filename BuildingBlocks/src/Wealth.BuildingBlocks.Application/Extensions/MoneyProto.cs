using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class MoneyProto
{
    public MoneyProto(CurrencyCode currency, decimal amount)
    {
        Currency = currency.ToProto();
        Amount = amount;
    }

    public static implicit operator Money(MoneyProto grpcMoney)
    {
        return new Money(grpcMoney.Currency.FromProto(), grpcMoney.Amount);
    }

    public static implicit operator MoneyProto(Money money)
    {
        return new MoneyProto(money.Currency, money.Amount);
    }
}
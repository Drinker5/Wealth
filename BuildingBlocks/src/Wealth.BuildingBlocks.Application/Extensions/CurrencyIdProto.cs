using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class CurrencyIdProto
{
    public CurrencyIdProto(string code)
    {
        Code = code;
    }

    public static implicit operator CurrencyId(CurrencyIdProto grpcMoney)
    {
        return new CurrencyId(grpcMoney.Code);
    }

    public static implicit operator CurrencyIdProto(CurrencyId value)
    {
        return new CurrencyIdProto(value.Code);
    }

    public static implicit operator CurrencyIdProto(string value)
    {
        return new CurrencyIdProto(value);
    }
}
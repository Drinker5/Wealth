using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class CurrencyIdProto
{
    public CurrencyIdProto(CurrencyCode code)
    {
        Code = (CurrencyCodeProto)code;
    }

    public CurrencyIdProto(CurrencyCodeProto code)
    {
        Code = code;
    }
    
    public static implicit operator CurrencyId(CurrencyIdProto grpcMoney)
    {
        return new CurrencyId((byte)grpcMoney.Code);
    }

    public static implicit operator CurrencyIdProto(CurrencyId id)
    {
        return new CurrencyIdProto(id.Value);
    }

    public static implicit operator CurrencyIdProto(CurrencyCode code)
    {
        return new CurrencyIdProto(code);
    }
    
    public static implicit operator CurrencyIdProto(CurrencyCodeProto code)
    {
        return new CurrencyIdProto(code);
    }
}
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement;

public partial class CurrencyIdProto
{
    public CurrencyIdProto(string code)
    {
        Code = code;
    }

    public static implicit operator CurrencyId(Wealth.InstrumentManagement.CurrencyIdProto grpcMoney)
    {
        return new CurrencyId(grpcMoney.Code);
    }

    public static implicit operator Wealth.InstrumentManagement.CurrencyIdProto(CurrencyId value)
    {
        return new CurrencyIdProto(value.Code);
    }
}
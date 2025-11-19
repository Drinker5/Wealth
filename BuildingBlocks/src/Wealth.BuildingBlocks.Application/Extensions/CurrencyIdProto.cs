using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class CurrencyIdProto
{
    public CurrencyIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator CurrencyId(CurrencyIdProto grpcValue)
    {
        return new CurrencyId(grpcValue.Id);
    }

    public static implicit operator CurrencyIdProto(CurrencyId value)
    {
        return new CurrencyIdProto(value.Value);
    }
}
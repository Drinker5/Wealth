using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class BondIdProto
{
    public BondIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator BondId(BondIdProto grpcValue)
    {
        return new BondId(grpcValue.Id);
    }

    public static implicit operator BondIdProto(BondId value)
    {
        return new BondIdProto(value.Value);
    }
}
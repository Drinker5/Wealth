using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class StockIdProto
{
    public StockIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator StockId(StockIdProto grpcValue)
    {
        return new StockId(grpcValue.Id);
    }

    public static implicit operator StockIdProto(StockId value)
    {
        return new StockIdProto(value.Value);
    }
}
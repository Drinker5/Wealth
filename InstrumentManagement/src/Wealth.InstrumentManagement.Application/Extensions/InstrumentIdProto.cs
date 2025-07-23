using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class InstrumentIdProto
{
    public InstrumentIdProto(Guid id)
    {
        Id = id;
    }
    
    public static implicit operator InstrumentId(InstrumentIdProto grpcValue)
    {
        return new InstrumentId(grpcValue.Id);
    }

    public static implicit operator InstrumentIdProto(InstrumentId value)
    {
        return new InstrumentIdProto(value.Id);
    }
}
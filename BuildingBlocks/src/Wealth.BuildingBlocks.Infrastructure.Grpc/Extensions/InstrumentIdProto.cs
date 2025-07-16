using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement;

public partial class InstrumentIdProto
{
    public InstrumentIdProto(Guid id)
    {
        Id = id;
    }
    
    public static implicit operator InstrumentId(Wealth.InstrumentManagement.InstrumentIdProto grpcValue)
    {
        return new InstrumentId(grpcValue.Id);
    }

    public static implicit operator Wealth.InstrumentManagement.InstrumentIdProto(InstrumentId value)
    {
        return new InstrumentIdProto(value.Id);
    }
}
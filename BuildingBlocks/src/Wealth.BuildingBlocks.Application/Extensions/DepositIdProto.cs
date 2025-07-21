using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public partial class DepositIdProto
{
    public DepositIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator DepositId(DepositIdProto grpcValue)
    {
        return new DepositId(grpcValue.Id);
    }

    public static implicit operator DepositIdProto(DepositId value)
    {
        return new DepositIdProto(value.Id);
    }
}
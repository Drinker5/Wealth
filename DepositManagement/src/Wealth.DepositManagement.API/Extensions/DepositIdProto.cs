using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.BuildingBlocks.DepositManagement;

public partial class DepositIdProto
{
    public DepositIdProto(int value)
    {
        Id = value;
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
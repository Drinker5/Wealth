using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.BuildingBlocks;

public partial class WalletIdProto
{
    public WalletIdProto(int id)
    {
        Id = id;
    }
    
    public static implicit operator WalletId(WalletIdProto grpcValue)
    {
        return new WalletId(grpcValue.Id);
    }

    public static implicit operator WalletIdProto(WalletId value)
    {
        return new WalletIdProto(value.Id);
    }
}
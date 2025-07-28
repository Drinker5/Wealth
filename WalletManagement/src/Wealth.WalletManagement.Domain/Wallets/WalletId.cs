using Wealth.BuildingBlocks.Domain;

namespace Wealth.WalletManagement.Domain.Wallets;

public record struct WalletId(int Id) : IIdentity
{
    public static WalletId New() => new WalletId(0);

    public override string ToString()
    {
        return Id.ToString();
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
    public static implicit operator int(WalletId walletId)
    {
        return walletId.Id;
    }
    
    public static implicit operator WalletId(int id)
    {
        return new WalletId(id);
    }
}
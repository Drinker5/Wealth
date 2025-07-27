using Wealth.BuildingBlocks.Domain;

namespace Wealth.WalletManagement.Domain.Wallets;

public record struct WalletId(int Id) : IIdentity
{
    public static WalletId New() => new WalletId(0);
}
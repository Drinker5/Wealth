using Wealth.BuildingBlocks.Domain;

namespace Wealth.WalletManagement.Domain.Wallets;

public record WalletRenamed(WalletId WalletId, string NewName) : DomainEvent;
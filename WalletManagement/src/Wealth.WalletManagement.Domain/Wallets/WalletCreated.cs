using Wealth.BuildingBlocks.Domain;

namespace Wealth.WalletManagement.Domain.Wallets;

public record WalletCreated(Wallet Wallet) : DomainEvent;
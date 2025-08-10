using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.Wallets;

public record WalletRenamed(WalletId WalletId, string NewName) : DomainEvent;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.Wallets;

public record WalletMoneyEjected(WalletId WalletId, Money Money) : DomainEvent;
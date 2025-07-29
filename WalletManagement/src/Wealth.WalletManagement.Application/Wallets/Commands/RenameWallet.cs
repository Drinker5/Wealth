using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public record RenameWallet(WalletId WalletId, string NewName) : ICommand;
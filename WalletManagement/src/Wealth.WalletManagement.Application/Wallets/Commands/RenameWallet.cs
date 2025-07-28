using Wealth.BuildingBlocks.Application;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public record RenameWallet(string NewName) : ICommand;
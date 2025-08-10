using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public record InsertMoney(WalletId WalletId, Money Money) : ICommand;
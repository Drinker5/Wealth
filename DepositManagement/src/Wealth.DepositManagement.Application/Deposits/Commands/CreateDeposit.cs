using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public record CreateDeposit(string Name, Yield Yield, CurrencyId CurrencyId) : ICommand<DepositId>;
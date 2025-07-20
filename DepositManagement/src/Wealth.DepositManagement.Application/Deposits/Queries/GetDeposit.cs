using Wealth.BuildingBlocks.Application;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Application.Deposits.Queries;

public record GetDeposit(DepositId DepositId) : IQuery<Deposit?>;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.DepositOperations;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public record CreateDepositOperation(
    DepositId DepositId,
    DepositOperationType Type,
    DateTimeOffset Date,
    Money Money) : ICommand;
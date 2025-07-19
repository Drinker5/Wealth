using Wealth.BuildingBlocks.Domain;

namespace Wealth.DepositManagement.Domain.Deposits.Events;

public record DepositYieldChanged(DepositId DepositId, Yield Yield) : IDomainEvent;
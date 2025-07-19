using Wealth.BuildingBlocks.Domain;

namespace Wealth.DepositManagement.Domain.Deposits.Events;

public record DepositRenamed(DepositId DepositId, string Name) : IDomainEvent;
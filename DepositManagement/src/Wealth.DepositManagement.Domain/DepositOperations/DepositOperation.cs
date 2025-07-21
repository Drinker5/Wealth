using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Domain.DepositOperations;

public class DepositOperation : IEntity
{
    public Guid Id { get; set; }
    public DepositId DepositId { get; set; }
    public DepositOperationType Type { get; set; }
    public DateTimeOffset Date { get; set; }
    public Money Money { get; set; }
}
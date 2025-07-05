using System.Data;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Application.Outbox;

public interface IOutboxRepository
{
    Task Add(IDomainEvent domainEvent);
}

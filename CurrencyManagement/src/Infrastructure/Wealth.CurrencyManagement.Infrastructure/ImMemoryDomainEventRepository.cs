using Wealth.CurrencyManagement.Domain.Interfaces;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Infrastructure;

public class ImMemoryDomainEventRepository : IDomainEventRepository
{
    public Task Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IDomainEvent>> FindAll()
    {
        throw new NotImplementedException();
    }
}
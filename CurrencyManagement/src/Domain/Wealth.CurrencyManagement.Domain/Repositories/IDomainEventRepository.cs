using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Repositories;

public interface IDomainEventRepository
{
    Task Add<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
    Task<IEnumerable<IDomainEvent>> FindAll();
}


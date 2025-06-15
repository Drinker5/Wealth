using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Entities;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : IDomainEvent;
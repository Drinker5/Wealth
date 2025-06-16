using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Currency;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : IDomainEvent;
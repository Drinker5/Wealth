using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : IDomainEvent;
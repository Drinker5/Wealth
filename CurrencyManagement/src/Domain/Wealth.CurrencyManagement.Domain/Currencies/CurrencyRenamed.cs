using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : IDomainEvent;
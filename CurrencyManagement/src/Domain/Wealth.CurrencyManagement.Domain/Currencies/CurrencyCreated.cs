using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyCreated(CurrencyId CurrencyId, string Name, string Symbol) : IDomainEvent;
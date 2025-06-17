using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyCreated(CurrencyId CurrencyId, string Name, string Symbol) : IDomainEvent;
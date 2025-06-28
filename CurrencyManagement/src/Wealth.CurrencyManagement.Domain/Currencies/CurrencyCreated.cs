using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyCreated(CurrencyId CurrencyId, string Name, string Symbol) : IDomainEvent;
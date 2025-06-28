using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : IDomainEvent;
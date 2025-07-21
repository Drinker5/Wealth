using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyRenamed(CurrencyId CurrencyId, string NewName) : DomainEvent;
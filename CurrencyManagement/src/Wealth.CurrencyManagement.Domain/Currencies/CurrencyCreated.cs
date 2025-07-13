using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Domain.Currencies;

public record CurrencyCreated(CurrencyId CurrencyId, string Name, string Symbol) : IDomainEvent;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public record CurrencyPriceChanged(CurrencyId CurrencyId, Money NewPrice) : DomainEvent;
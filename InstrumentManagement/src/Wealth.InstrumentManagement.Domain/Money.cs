using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
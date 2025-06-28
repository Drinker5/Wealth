using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record Dividend(CurrencyId CurrencyId, decimal ValuePerYear) : IValueObject;
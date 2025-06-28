using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public record Coupon(CurrencyId CurrencyId, decimal ValuePerYear) : IValueObject;
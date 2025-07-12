using Wealth.BuildingBlocks.Domain;

namespace Wealth.PortfolioManagement.Domain.ValueObjects;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
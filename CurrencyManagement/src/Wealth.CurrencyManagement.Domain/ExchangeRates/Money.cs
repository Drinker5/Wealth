using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Domain.ExchangeRates;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
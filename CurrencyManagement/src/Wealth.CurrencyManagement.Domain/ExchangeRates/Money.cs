using Wealth.BuildingBlocks.Domain;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Domain.ExchangeRates;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
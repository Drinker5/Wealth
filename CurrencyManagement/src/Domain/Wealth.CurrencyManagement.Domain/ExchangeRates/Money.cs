using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.ExchangeRates;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
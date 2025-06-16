using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.ExchangeRate;

public record Money(CurrencyId CurrencyId, decimal Amount) : IValueObject;
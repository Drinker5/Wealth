using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Query;

public record ExchangeQuery(
    Money Money,
    CurrencyId TargetCurrencyId,
    DateTime Date) : IQuery<Money?>;
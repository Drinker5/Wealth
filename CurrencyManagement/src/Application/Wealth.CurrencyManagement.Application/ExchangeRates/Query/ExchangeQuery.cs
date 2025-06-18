using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Query;

public record ExchangeQuery(
    Money Money,
    CurrencyId TargetCurrencyId,
    DateOnly Date) : IQuery<Money?>;
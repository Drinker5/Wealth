using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record ProvideNewExchangeRateCommand(CurrencyId BaseCurrencyId, CurrencyId TargetCurrencyId, DateOnly OnDate) : ICommand;
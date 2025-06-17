using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CreateExchangeRateCommand(
    CurrencyId BaseCurrencyId,
    CurrencyId TargetCurrencyId,
    decimal ExchangeRate,
    DateTime ValidOnDate) : ICommand;
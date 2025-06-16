using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Application.ExchangeRate.Commands;

public record CreateExchangeRateCommand(
    CurrencyId BaseCurrencyId,
    CurrencyId TargetCurrencyId,
    decimal ExchangeRate,
    DateTime ValidOnDate) : ICommand;
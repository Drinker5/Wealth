using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CheckNewExchangeRatesCommand(
    CurrencyId FromCurrency,
    CurrencyId ToCurrency) : ICommand;
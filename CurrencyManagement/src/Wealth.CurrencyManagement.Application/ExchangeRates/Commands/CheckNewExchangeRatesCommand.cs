using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CheckNewExchangeRatesCommand(
    CurrencyId FromCurrency,
    CurrencyId ToCurrency) : ICommand;
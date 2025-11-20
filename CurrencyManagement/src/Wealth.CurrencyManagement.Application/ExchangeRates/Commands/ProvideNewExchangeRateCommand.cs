using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record ProvideNewExchangeRateCommand(
    CurrencyCode BaseCurrency,
    CurrencyCode TargetCurrency,
    DateOnly OnDate) : ICommand;
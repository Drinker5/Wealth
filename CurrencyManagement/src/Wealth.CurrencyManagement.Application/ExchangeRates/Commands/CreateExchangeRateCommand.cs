using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CreateExchangeRateCommand(
    CurrencyCode BaseCurrency,
    CurrencyCode TargetCurrency,
    decimal ExchangeRate,
    DateOnly ValidOnDate) : ICommand;
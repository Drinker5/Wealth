using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CreateExchangeRateCommand(
    CurrencyId BaseCurrencyId,
    CurrencyId TargetCurrencyId,
    decimal ExchangeRate,
    DateOnly ValidOnDate) : ICommand;
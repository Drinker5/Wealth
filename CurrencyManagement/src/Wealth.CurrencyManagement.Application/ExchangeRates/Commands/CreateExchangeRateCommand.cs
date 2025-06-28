using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record CreateExchangeRateCommand(
    CurrencyId BaseCurrencyId,
    CurrencyId TargetCurrencyId,
    decimal ExchangeRate,
    DateOnly ValidOnDate) : ICommand;
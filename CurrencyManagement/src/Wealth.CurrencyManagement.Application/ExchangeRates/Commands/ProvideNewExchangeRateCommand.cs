using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

public record ProvideNewExchangeRateCommand(CurrencyId BaseCurrencyId, CurrencyId TargetCurrencyId, DateOnly OnDate) : ICommand;
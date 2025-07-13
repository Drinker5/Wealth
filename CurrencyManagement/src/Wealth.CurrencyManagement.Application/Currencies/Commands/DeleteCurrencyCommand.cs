using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record DeleteCurrencyCommand(CurrencyId CurrencyId) : ICommand;
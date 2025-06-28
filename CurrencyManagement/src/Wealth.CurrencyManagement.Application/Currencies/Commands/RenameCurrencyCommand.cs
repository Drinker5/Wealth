using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record RenameCurrencyCommand(CurrencyId CurrencyId, string NewName) : ICommand;
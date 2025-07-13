using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record RenameCurrencyCommand(CurrencyId CurrencyId, string NewName) : ICommand;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record CreateCurrencyCommand(CurrencyId CurrencyId, string Name, string Symbol) : ICommand<CurrencyDTO>;
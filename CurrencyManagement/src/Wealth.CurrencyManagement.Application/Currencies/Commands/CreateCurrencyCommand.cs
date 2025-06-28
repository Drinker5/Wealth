using Wealth.BuildingBlocks.Application;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record CreateCurrencyCommand(CurrencyId CurrencyId, string Name, string Symbol) : ICommand<CurrencyDTO>;
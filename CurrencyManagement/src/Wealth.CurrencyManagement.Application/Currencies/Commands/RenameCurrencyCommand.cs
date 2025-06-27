using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record RenameCurrencyCommand(CurrencyId CurrencyId, string NewName) : ICommand;
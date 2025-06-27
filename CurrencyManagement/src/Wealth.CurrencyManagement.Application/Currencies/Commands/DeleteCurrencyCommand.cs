using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;

namespace Wealth.CurrencyManagement.Application.Currencies.Commands;

public record DeleteCurrencyCommand(CurrencyId CurrencyId) : ICommand;
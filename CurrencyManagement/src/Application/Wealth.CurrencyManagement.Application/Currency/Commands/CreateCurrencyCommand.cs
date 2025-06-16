using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Currency;

namespace Wealth.CurrencyManagement.Application.Currency.Commands;

public record CreateCurrencyCommand(CurrencyId CurrencyId, string Name, string Symbol) : ICommand;
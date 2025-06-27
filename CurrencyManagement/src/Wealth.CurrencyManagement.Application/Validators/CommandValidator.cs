using FluentValidation;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Application.Validators;

public abstract class CommandValidator<T> : AbstractValidator<T>
    where T : ICommand
{
}

using FluentValidation;
using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Validators;

public abstract class CommandValidator<T> : AbstractValidator<T>
    where T : ICommand
{
}

using FluentValidation;

namespace Wealth.BuildingBlocks.Application.Validators;

public abstract class CommandValidator<T> : AbstractValidator<T>
    where T : ICommand
{
}

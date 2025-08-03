using FluentValidation;

namespace Wealth.BuildingBlocks.Application.Validators;

public abstract class QueryValidator<T> : AbstractValidator<T>
    where T : IQuery
{
}
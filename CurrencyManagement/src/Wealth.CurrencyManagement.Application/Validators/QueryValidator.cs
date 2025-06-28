using FluentValidation;
using Wealth.BuildingBlocks.Application;

namespace Wealth.CurrencyManagement.Application.Validators;

public abstract class QueryValidator<T> : AbstractValidator<T>
    where T : IQuery
{
}
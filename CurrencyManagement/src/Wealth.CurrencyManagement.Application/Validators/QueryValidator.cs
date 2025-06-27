using FluentValidation;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Application.Validators;

public abstract class QueryValidator<T> : AbstractValidator<T>
    where T : IQuery
{
}
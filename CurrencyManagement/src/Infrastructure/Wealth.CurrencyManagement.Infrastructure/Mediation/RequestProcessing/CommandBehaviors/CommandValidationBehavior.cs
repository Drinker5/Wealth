using System.Text;
using FluentValidation;
using MediatR;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errorMessages = GetErrorMessages(request);

        if (errorMessages.Any())
            ThrowException(errorMessages);

        return next(cancellationToken);
    }

    private IEnumerable<string> GetErrorMessages(TRequest request)
    {
        return validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .Select(x => x.ErrorMessage);
    }

    private static void ThrowException(IEnumerable<string> errorMessages)
    {
        var builder = new StringBuilder("InvalidCommand Reason: ");
        builder.Append(Environment.NewLine);
        builder.Append(Environment.NewLine);
        builder.Append(string.Join(Environment.NewLine, errorMessages));
        throw new ValidationException(builder.ToString());
    }
}

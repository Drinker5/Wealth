using MediatR;
using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

internal class CommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger;
    private readonly IJsonSerializer jsonSerializer;

    public CommandLoggingBehavior(
        ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger,
        IJsonSerializer jsonSerializer)
    {
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetType().Name, request);
        try
        {
            var response = await next(cancellationToken);
            logger.LogInformation("Command {CommandName} handled - response: {@Response}", request.GetType().Name, response);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw;
        }
    }
}
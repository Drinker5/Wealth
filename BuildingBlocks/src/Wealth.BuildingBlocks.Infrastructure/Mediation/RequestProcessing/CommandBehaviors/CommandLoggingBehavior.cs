using MediatR;
using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Application;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

public class CommandLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger;

    public CommandLoggingBehavior(ILogger<CommandLoggingBehavior<TRequest, TResponse>> logger)
    {
        this.logger = logger;
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
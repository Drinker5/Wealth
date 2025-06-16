using MediatR;
using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.RequestProcessing.CommandPipelines;

internal class CommandLoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly ILogger logger;
    private readonly IJsonSerializer jsonSerializer;

    public CommandLoggingPipeline(
        ILogger logger,
        IJsonSerializer jsonSerializer)
    {
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("{requestName} is processing: {environment}{request}",
            request.GetType().Name,
            Environment.NewLine,
            jsonSerializer.SerializeIndented(request));
        try
        {
            TResponse result = await next(cancellationToken);
            if (typeof(TResponse) != typeof(Unit))
            {
                logger.LogInformation("Result: {environment}{result}",
                    Environment.NewLine,
                    result);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.ToString());
            throw;
        }
        finally
        {
            logger.LogInformation("request {Name} is processed.", request.GetType().Name);
        }
    }
}
using MediatR;
using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.QueryPipelines;

internal class QueryLoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IQuery
{
    private readonly ILogger logger;
    private readonly IJsonSerializer jsonSerializer;

    public QueryLoggingPipeline(
        ILogger<QueryLoggingPipeline<TRequest, TResponse>> logger,
        IJsonSerializer jsonSerializer)
    {
        this.logger = logger;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Name} is processing: {@Request}", request.GetType().Name, request);
        try
        {
            TResponse result = await next(cancellationToken);
            if (typeof(TResponse) != typeof(Unit))
            {
                logger.LogInformation("Result: {@Request}", result);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Unhandled Exception:{NewLine}{Exception}", Environment.NewLine, ex);
            throw;
        }
        finally
        {
            logger.LogDebug("{Name} is processed.", request.GetType().Name);
        }
    }
}
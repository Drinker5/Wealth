using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;

internal class LoggingUnitOfWorkDecorator : IUnitOfWork
{
    private readonly IUnitOfWork decorated;
    private readonly ILogger<LoggingUnitOfWorkDecorator> logger;
    private bool commited;

    public LoggingUnitOfWorkDecorator(
        IUnitOfWork decorated,
        ILogger<LoggingUnitOfWorkDecorator> logger)
    {
        this.decorated = decorated;
        this.logger = logger;
    }

    public async Task<int> Commit(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Commiting changes...");
        if (commited)
            throw new Exception("UoW can not be commited twice within a scope.");

        var changes = await decorated.Commit(cancellationToken);
        logger.LogInformation("{Changes} changes just commited", changes);

        commited = true;
        return changes;
    }
}

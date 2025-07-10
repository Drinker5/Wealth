using Microsoft.Extensions.Logging;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.Decorators;

internal class UnitOfWorkLoggingDecorator : IUnitOfWork
{
    private readonly IUnitOfWork decorated;
    private readonly ILogger<UnitOfWorkLoggingDecorator> logger;
    private bool commited;

    public UnitOfWorkLoggingDecorator(
        IUnitOfWork decorated,
        ILogger<UnitOfWorkLoggingDecorator> logger)
    {
        this.decorated = decorated;
        this.logger = logger;
    }

    public Task<IDisposable> BeginTransaction() => decorated.BeginTransaction();
    
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

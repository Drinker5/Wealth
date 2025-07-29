using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Extensions;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

internal class CommandDispatchEventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IMediator mediator;
    private readonly WealthDbContext dbContext;

    public CommandDispatchEventsBehavior(IMediator mediator, WealthDbContext dbContext)
    {
        this.mediator = mediator;
        this.dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);
        await mediator.DispatchDomainEvents(dbContext);
        return result;
    }
}
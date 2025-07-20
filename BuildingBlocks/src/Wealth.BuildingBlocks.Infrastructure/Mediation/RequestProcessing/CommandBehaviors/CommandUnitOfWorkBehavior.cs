using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

public class CommandUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IUnitOfWork unitOfWork;

    public CommandUnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransaction();
        var result = await next(cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        return result;
    }
}
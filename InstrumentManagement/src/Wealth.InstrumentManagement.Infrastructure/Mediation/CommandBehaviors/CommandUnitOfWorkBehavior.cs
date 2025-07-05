using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Mediation.CommandBehaviors;

public class CommandUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly UnitOfWork unitOfWork;

    public CommandUnitOfWorkBehavior(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction();
        var result = await next(cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        return result;
    }
}
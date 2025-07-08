using MediatR;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.CurrencyManagement.Infrastructure.Mediation.RequestProcessing.CommandBehaviors;

internal class CommandUnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IUnitOfWork unitOfWork;

    public CommandUnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var result = await next(cancellationToken);
        await unitOfWork.Commit(cancellationToken);
        return result;
    }
}
using MediatR;
using Wealth.CurrencyManagement.Application.Interfaces;
using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Infrastructure.RequestProcessing.CommandPipelines;

public class CommandUnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IUnitOfWork unitOfWork;

    public CommandUnitOfWorkPipeline(IUnitOfWork unitOfWork)
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
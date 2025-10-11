using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Application.Extensions;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddOperationHandler(
    IOperationRepository repository,
    IKafkaProducer producer) : ICommandHandler<AddOperation>
{
    public async Task Handle(AddOperation request, CancellationToken cancellationToken)
    {
        await repository.UpsertOperation(request.Operation, cancellationToken);
        await producer.ProduceAsync(
            "wealth-operations-converted",
            [
                new BusMessage<string, OperationProto>
                {
                    Key = request.Operation.Id.Value,
                    Value = request.Operation.ToProto(),
                }
            ],
            cancellationToken);
    }
}
using Microsoft.Extensions.Options;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement.Application.Extensions;
using Wealth.PortfolioManagement.Application.Options;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Portfolios.Commands;

public class AddOperationHandler(
    IOperationRepository repository,
    IOptions<OperationsProducerOptions> producerOptions,
    IKafkaProducer producer) : ICommandHandler<AddOperation>
{
    public async Task Handle(AddOperation request, CancellationToken cancellationToken)
    {
        await repository.UpsertOperation(request.Operation, cancellationToken);
        await producer.Produce(
            producerOptions.Value.Topic,
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
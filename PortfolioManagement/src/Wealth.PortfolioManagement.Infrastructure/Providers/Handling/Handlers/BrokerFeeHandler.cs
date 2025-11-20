using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BrokerFeeHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        /*
         * fail: Wealth.BuildingBlocks.Infrastructure.KafkaConsumer.ConsumerHostedService[0]
2025-11-18T18:58:32.384249718Z       wealth-operations: Retry attempt 9. Next retry in 00:04:16
2025-11-18T18:58:32.384258218Z       System.ArgumentOutOfRangeException: Specified argument was out of the range of valid values.
2025-11-18T18:58:32.384262918Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers.BrokerFeeHandler.Handle(Operation operation, InstrumentType instrumentType, PortfolioId portfolioId)+MoveNext() in E:\Coding\Wealth\PortfolioManagement\src\Wealth.PortfolioManagement.Infrastructure\Providers\Handling\Handlers\BrokerFeeHandler.cs:line 33
2025-11-18T18:58:32.384288519Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers.BrokerFeeHandler.Handle(Operation operation, InstrumentType instrumentType, PortfolioId portfolioId)+System.Threading.Tasks.Sources.IValueTaskSource<System.Boolean>.GetResult()
2025-11-18T18:58:32.384295520Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.OperationConverter.ConvertOperation(Operation operation, PortfolioId portfolioId)+MoveNext() in E:\Coding\Wealth\PortfolioManagement\src\Wealth.PortfolioManagement.Infrastructure\Providers\Handling\OperationConverter.cs:line 44
2025-11-18T18:58:32.384300020Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.OperationConverter.ConvertOperation(Operation operation, PortfolioId portfolioId)+MoveNext() in E:\Coding\Wealth\PortfolioManagement\src\Wealth.PortfolioManagement.Infrastructure\Providers\Handling\OperationConverter.cs:line 44
2025-11-18T18:58:32.384314220Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.OperationConverter.ConvertOperation(Operation operation, PortfolioId portfolioId)+System.Threading.Tasks.Sources.IValueTaskSource<System.Boolean>.GetResult()
2025-11-18T18:58:32.384316821Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.OperationHandler.Handle(Operation operation, CancellationToken token) in E:\Coding\Wealth\PortfolioManagement\src\Wealth.PortfolioManagement.Infrastructure\Providers\Handling\OperationHandler.cs:line 18
2025-11-18T18:58:32.384320021Z          at Wealth.PortfolioManagement.Infrastructure.Providers.Handling.OperationHandler.Handle(Operation operation, CancellationToken token) in E:\Coding\Wealth\PortfolioManagement\src\Wealth.PortfolioManagement.Infrastructure\Providers\Handling\OperationHandler.cs:line 18
2025-11-18T18:58:32.384322421Z          at Wealth.BuildingBlocks.Infrastructure.KafkaConsumer.ConsumerHostedService`1.<Handle>b__7_0(T t, CancellationToken ct) in E:\Coding\Wealth\BuildingBlocks\src\Wealth.BuildingBlocks.Infrastructure.KafkaConsumer\ConsumerHostedService.cs:line 47
2025-11-18T18:58:32.384324921Z          at Polly.ResiliencePipeline.<>c__2`1.<<ExecuteAsync>b__2_0>d.MoveNext()
2025-11-18T18:59:17.486093029Z 
         */
        yield return instrumentType switch
        {
            InstrumentType.Bond => new BondBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            InstrumentType.Share => new StockBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            InstrumentType.Currency => new CurrencyBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                CurrencyId = await instrumentIdProvider.GetCurrencyIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
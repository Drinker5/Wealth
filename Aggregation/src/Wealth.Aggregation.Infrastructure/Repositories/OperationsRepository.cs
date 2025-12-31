using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class OperationsRepository(ITableWriterBuilder tableWriterBuilder) : IOperationsRepository
{
    private readonly ITableWriter<Operation> _tableWriter = tableWriterBuilder
        .For<Operation>("operations")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("instrument_id", i => i.InstrumentIdType.Id.Value)
        .AddColumn("instrument_type", i => (byte)i.InstrumentIdType.Type)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("quantity", i => i.Quantity)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("operation_type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(IEnumerable<Operation> operations, CancellationToken token)
    {
        await _tableWriter.Insert(operations, token);
    }
}
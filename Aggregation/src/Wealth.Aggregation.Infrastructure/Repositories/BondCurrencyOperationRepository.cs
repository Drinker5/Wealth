using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class BondCurrencyOperationRepository(ITableWriterBuilder tableWriterBuilder) : IBondCurrencyOperationRepository
{
    private readonly ITableWriter<BondCurrencyOperation> _tableWriter = tableWriterBuilder
        .For<BondCurrencyOperation>("bond_currency_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("bond_id", i => i.BondId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(BondCurrencyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}
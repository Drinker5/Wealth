using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class CurrencyMoneyOperationRepository(ITableWriterBuilder tableWriterBuilder) : ICurrencyMoneyOperationRepository
{
    private readonly ITableWriter<CurrencyMoneyOperation> _tableWriter = tableWriterBuilder
        .For<CurrencyMoneyOperation>("currency_money_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("currency_id", i => i.CurrencyId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(CurrencyMoneyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}
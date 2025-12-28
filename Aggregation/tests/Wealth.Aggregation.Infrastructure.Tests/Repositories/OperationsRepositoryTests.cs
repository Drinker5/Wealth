using AutoFixture;
using JetBrains.Annotations;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(OperationsRepository))]
public class OperationsRepositoryTests : IClassFixture<ClickHouseFixture>
{
    private readonly ITestOutputHelper output;
    private readonly ClickHouseFixture clickHouseFixture;
    private readonly OperationsRepository operationsRepository;
    private readonly Fixture fixture;

    public OperationsRepositoryTests(ITestOutputHelper output, ClickHouseFixture clickHouseFixture)
    {
        fixture = new Fixture();
        this.output = output;
        this.clickHouseFixture = clickHouseFixture;

        var clickHouseConnectionStringBuilder = new ClickHouseConnectionStringBuilder(clickHouseFixture.ClickHouseConnectionString);
        var clickHouseConnectionSettings = clickHouseConnectionStringBuilder.BuildSettings();
        var connectionFactory = new ClickHouseConnectionFactory(clickHouseConnectionSettings);
        operationsRepository = new OperationsRepository(new TableWriterBuilder(connectionFactory));
    }

    [Fact]
    public async Task WhenUpsert()
    {
        var logs = await clickHouseFixture.migratorContainer.GetLogsAsync();
        output.WriteLine(logs.Stdout);
        output.WriteLine(logs.Stderr);

        var ops = fixture.CreateMany<Operation>(10).ToArray();

        await operationsRepository.Upsert(ops, CancellationToken.None);

        await CheckOps(ops, CancellationToken.None);
    }

    private async Task CheckOps(IReadOnlyCollection<Operation> ops, CancellationToken token)
    {
        await using var connection = new ClickHouseConnection(clickHouseFixture.ClickHouseConnectionString);
        await connection.OpenAsync(token);

        var command = connection.CreateCommand("select * from operations");
        var reader = await command.ExecuteReaderAsync(token);

        var ids = ops.Select(o => o.Id).ToHashSet();
        while (await reader.ReadAsync(token))
        {
            var id = reader.GetString(0);

            Assert.Contains(id, ids);
        }
    }
}
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

namespace Wealth.Aggregation.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.Aggregation.Infrastructure --startup-project .\src\Wealth.Aggregation.API Name
/// dotnet ef database update --project src\Wealth.Aggregation.Infrastructure --startup-project .\src\Wealth.Aggregation.API
/// </summary>
public class WealthDbContext : DbContext, IDesignTimeDbContextFactory<WealthDbContext>, IUnitOfWork
{
    public virtual DbSet<StockAggregation> StockAggregations { get; internal init; }

    private IDbContextTransaction? transaction;

    public WealthDbContext()
    {
    }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public WealthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<CurrencyId>().HaveConversion<CurrencyIdConverter>();
        configurationBuilder.Properties<InstrumentId>().HaveConversion<InstrumentIdConverter>();
    }

    public async Task<IDisposable> BeginTransaction()
    {
        if (transaction != null)
            return transaction;

        transaction = await Database.BeginTransactionAsync();
        return transaction;
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        var result = await SaveChangesAsync(cancellationToken);
        if (transaction != null)
            await transaction.CommitAsync(cancellationToken);

        return result;
    }
}
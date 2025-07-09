using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Wealth.CurrencyManagement.Infrastructure --startup-project .\src\Wealth.CurrencyManagement.API Name
/// dotnet ef database update --project src\Wealth.CurrencyManagement.Infrastructure --startup-project .\src\Wealth.CurrencyManagement.API
/// </summary>
public class WealthDbContext : DbContext, IDesignTimeDbContextFactory<WealthDbContext>
{
    public virtual DbSet<Currency> Currencies { get; internal init; }
    public virtual DbSet<ExchangeRate> ExchangeRates { get; internal init; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; internal init; }


    private bool commited;
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

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (commited)
            throw new Exception("can not commit twice within a scope in DbContext");

        commited = true;
        return base.SaveChangesAsync(cancellationToken);
    }

    public WealthDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseInMemoryDatabase("Design");
        return new WealthDbContext(optionsBuilder.Options);
    }
}
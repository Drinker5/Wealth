using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project src\Infrastructure\Wealth.CurrencyManagement.Infrastructure --startup-project .\src\API\Wealth.CurrencyManagement.API Name
/// dotnet ef database update --project src\Infrastructure\Wealth.CurrencyManagement.Infrastructure --startup-project .\src\API\Wealth.CurrencyManagement.API
/// </summary>
public class WealthDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; internal init; }
    public DbSet<ExchangeRate> ExchangeRates { get; internal init; }
    public DbSet<OutboxMessage> OutboxMessages { get; internal init; }

    private bool commited;

    public WealthDbContext(DbContextOptions options)
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
}
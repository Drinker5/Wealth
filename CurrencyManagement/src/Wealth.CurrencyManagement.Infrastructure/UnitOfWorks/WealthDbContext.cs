using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Infrastructure.EFCore;
using Wealth.CurrencyManagement.Domain.ExchangeRates;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

/// <summary>
/// dotnet ef migrations add --project .\CurrencyManagement\src\Wealth.CurrencyManagement.Infrastructure --startup-project .\CurrencyManagement\src\Wealth.CurrencyManagement.API Name
/// dotnet ef database update --project .\CurrencyManagement\src\Wealth.CurrencyManagement.Infrastructure --startup-project .\CurrencyManagement\src\Wealth.CurrencyManagement.API
/// </summary>
public class WealthDbContext : WealthDbContextBase
{
    public virtual DbSet<ExchangeRate> ExchangeRates { get; internal init; }
    public virtual DbSet<DefferedCommand> DefferedCommands { get; internal init; }


    private bool commited;
    public WealthDbContext()
    {
    }

    public WealthDbContext(DbContextOptions<WealthDbContext> options)
        : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (commited)
            throw new Exception("can not commit twice within a scope in DbContext");

        commited = true;
        return base.SaveChangesAsync(cancellationToken);
    }

    public override WealthDbContextBase CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WealthDbContext>();
        optionsBuilder.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=postgres;Database=Design");
        return new WealthDbContext(optionsBuilder.Options);
    }
}
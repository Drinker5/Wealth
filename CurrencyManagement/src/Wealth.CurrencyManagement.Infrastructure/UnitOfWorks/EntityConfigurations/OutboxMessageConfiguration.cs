using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wealth.CurrencyManagement.Application.Abstractions;

namespace Wealth.CurrencyManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id);
        builder.Property(x => x.Data);
        builder.Property(x => x.OccurredOn);
        builder.Property(x => x.ProcessedDate);
        builder.Property(x => x.Type);
        builder.Property(x => x.AssemblyName);
        builder.Property(x => x.Error);

        builder.HasNoDiscriminator();
    }
}
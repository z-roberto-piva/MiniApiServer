using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Configurations;

public sealed class StatConfiguration : IEntityTypeConfiguration<Stat>
{
    public void Configure(EntityTypeBuilder<Stat> builder)
    {
        builder.ToTable("stats");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .HasColumnName("id");

        builder.Property(entity => entity.Date)
            .HasColumnName("date")
            .IsRequired();

        builder.Property(entity => entity.NumberOfOperations)
            .HasColumnName("n_of_operations")
            .IsRequired();

        builder.Property(entity => entity.TotalOfSums)
            .HasColumnName("total_of_sums")
            .IsRequired();

        builder.Property(entity => entity.TotalOfSubtractions)
            .HasColumnName("total_of_subtractions")
            .IsRequired();
    }
}

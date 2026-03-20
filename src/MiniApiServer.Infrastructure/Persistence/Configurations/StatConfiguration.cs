using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps <see cref="Stat"/> to the `stats` table.
/// </summary>
public sealed class StatConfiguration : IEntityTypeConfiguration<Stat>
{
    /// <summary>
    /// Configures the entity mapping.
    /// </summary>
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

        builder.Property(entity => entity.TotalOfMultiplications)
            .HasColumnName("total_of_multiplications")
            .IsRequired();

        builder.Property(entity => entity.TotalOfDivisions)
            .HasColumnName("total_of_divisions")
            .IsRequired();
    }
}

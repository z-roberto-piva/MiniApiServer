using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for the application data store.
/// </summary>
public sealed class MiniApiServerDbContext(DbContextOptions<MiniApiServerDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Default PostgreSQL schema used by the application tables.
    /// </summary>
    public const string Schema = "mini_api_server";

    /// <summary>
    /// Gets the set of input rows.
    /// </summary>
    public DbSet<DataIn> DataIns => Set<DataIn>();

    /// <summary>
    /// Gets the set of persisted sum results.
    /// </summary>
    public DbSet<DataSumm> DataSumms => Set<DataSumm>();

    /// <summary>
    /// Gets the set of persisted subtraction results.
    /// </summary>
    public DbSet<DataSubtraction> DataSubtractions => Set<DataSubtraction>();

    /// <summary>
    /// Gets the set of persisted multiplication results.
    /// </summary>
    public DbSet<DataMultiplication> DataMultiplications => Set<DataMultiplication>();

    /// <summary>
    /// Gets the set of persisted division results.
    /// </summary>
    public DbSet<DataDivision> DataDivisions => Set<DataDivision>();

    /// <summary>
    /// Gets the set of daily statistics rows.
    /// </summary>
    public DbSet<Stat> Stats => Set<Stat>();

    /// <summary>
    /// Applies entity mappings and the default schema.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MiniApiServerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

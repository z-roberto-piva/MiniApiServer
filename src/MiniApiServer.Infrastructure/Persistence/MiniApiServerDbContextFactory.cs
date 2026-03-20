using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniApiServer.Infrastructure.Persistence;

/// <summary>
/// Creates the EF Core context at design time for migrations and tooling.
/// </summary>
public sealed class MiniApiServerDbContextFactory : IDesignTimeDbContextFactory<MiniApiServerDbContext>
{
    /// <summary>
    /// Builds a design-time context using the configured environment connection string or a local fallback.
    /// </summary>
    public MiniApiServerDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("MINI_API_SERVER_APP_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=mini_api_server;Username=mini_api_server;Password=sviluppo";

        var optionsBuilder = new DbContextOptionsBuilder<MiniApiServerDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", MiniApiServerDbContext.Schema));

        return new MiniApiServerDbContext(optionsBuilder.Options);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MiniApiServer.Infrastructure.Persistence;

public sealed class MiniApiServerDbContextFactory : IDesignTimeDbContextFactory<MiniApiServerDbContext>
{
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

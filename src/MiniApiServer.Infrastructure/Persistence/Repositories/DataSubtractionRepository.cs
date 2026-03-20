using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;
using Npgsql;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IDataSubtractionRepository"/>.
/// </summary>
public sealed class DataSubtractionRepository(MiniApiServerDbContext dbContext) : IDataSubtractionRepository
{
    /// <inheritdoc />
    public async Task AddAsync(DataSubtraction dataSubtraction, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.DataSubtractions.AddAsync(dataSubtraction, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            dbContext.Entry(dataSubtraction).State = EntityState.Detached;
        }
    }
}

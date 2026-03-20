using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;
using Npgsql;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IDataDivisionRepository"/>.
/// </summary>
public sealed class DataDivisionRepository(MiniApiServerDbContext dbContext) : IDataDivisionRepository
{
    /// <inheritdoc />
    public async Task AddAsync(DataDivision dataDivision, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.DataDivisions.AddAsync(dataDivision, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            dbContext.Entry(dataDivision).State = EntityState.Detached;
        }
    }
}

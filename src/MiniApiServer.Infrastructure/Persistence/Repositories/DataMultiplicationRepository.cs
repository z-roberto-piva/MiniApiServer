using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;
using Npgsql;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class DataMultiplicationRepository(MiniApiServerDbContext dbContext) : IDataMultiplicationRepository
{
    public async Task AddAsync(DataMultiplication dataMultiplication, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.DataMultiplications.AddAsync(dataMultiplication, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            dbContext.Entry(dataMultiplication).State = EntityState.Detached;
        }
    }
}

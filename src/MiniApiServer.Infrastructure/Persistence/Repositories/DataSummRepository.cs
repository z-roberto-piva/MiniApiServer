using Microsoft.EntityFrameworkCore;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;
using Npgsql;

namespace MiniApiServer.Infrastructure.Persistence.Repositories;

public sealed class DataSummRepository(MiniApiServerDbContext dbContext) : IDataSummRepository
{
    public async Task AddAsync(DataSumm dataSumm, CancellationToken cancellationToken = default)
    {
        try
        {
            await dbContext.DataSumms.AddAsync(dataSumm, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            dbContext.Entry(dataSumm).State = EntityState.Detached;
        }
    }
}

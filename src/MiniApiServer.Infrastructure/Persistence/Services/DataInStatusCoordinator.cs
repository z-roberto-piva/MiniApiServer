using Microsoft.EntityFrameworkCore;
using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Domain.Entities;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Infrastructure.Persistence.Services;

public sealed class DataInStatusCoordinator(MiniApiServerDbContext dbContext) : IDataInStatusCoordinator
{
    public async Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var dataIn = await dbContext.DataIns
            .FromSqlInterpolated($"SELECT * FROM data_in WHERE id = {dataInId} FOR UPDATE")
            .SingleAsync(cancellationToken);

        var hasSumm = await dbContext.DataSumms.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);
        var hasSubtraction = await dbContext.DataSubtractions.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);

        if (hasSumm && hasSubtraction)
        {
            if (dataIn.Status != OperationStatus.DONE)
            {
                if (dataIn.Status == OperationStatus.TODO)
                {
                    dataIn.MarkAsDoing();
                }

                dataIn.MarkAsDone();
            }
        }
        else if ((hasSumm || hasSubtraction) && dataIn.Status == OperationStatus.TODO)
        {
            dataIn.MarkAsDoing();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task MarkProcessingStartedAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        var trackedDataIn = await dbContext.DataIns.SingleAsync(entity => entity.Id == dataIn.Id, cancellationToken);

        if (trackedDataIn.Status == OperationStatus.TODO)
        {
            trackedDataIn.MarkAsDoing();
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

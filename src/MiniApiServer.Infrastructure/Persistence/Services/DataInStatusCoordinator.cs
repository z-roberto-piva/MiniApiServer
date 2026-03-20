using Microsoft.EntityFrameworkCore;
using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Domain.Entities;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Infrastructure.Persistence.Services;

/// <summary>
/// Coordinates state transitions for <see cref="DataIn"/> rows based on completed background operations.
/// </summary>
public sealed class DataInStatusCoordinator(MiniApiServerDbContext dbContext) : IDataInStatusCoordinator
{
    /// <summary>
    /// Marks a single operation as completed and closes the input when every expected result exists.
    /// </summary>
    public async Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var dataIn = await dbContext.DataIns
            .FromSqlInterpolated($"SELECT * FROM data_in WHERE id = {dataInId} FOR UPDATE")
            .SingleAsync(cancellationToken);

        var hasSumm = await dbContext.DataSumms.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);
        var hasSubtraction = await dbContext.DataSubtractions.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);
        var hasMultiplication = await dbContext.DataMultiplications.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);
        var hasDivision = await dbContext.DataDivisions.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);

        if (hasSumm && hasSubtraction && hasMultiplication && hasDivision)
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
        else if ((hasSumm || hasSubtraction || hasMultiplication || hasDivision) && dataIn.Status == OperationStatus.TODO)
        {
            dataIn.MarkAsDoing();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Moves the specified input to the in-progress state when processing begins.
    /// </summary>
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

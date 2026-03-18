using Microsoft.EntityFrameworkCore;
using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Domain.Entities;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Infrastructure.Persistence.Services;

public sealed class DataInStatusCoordinator(MiniApiServerDbContext dbContext) : IDataInStatusCoordinator
{
    public async Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default)
    {
        var dataIn = await dbContext.DataIns.SingleAsync(entity => entity.Id == dataInId, cancellationToken);
        var hasSumm = await dbContext.DataSumms.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);
        var hasSubtraction = await dbContext.DataSubtractions.AnyAsync(entity => entity.DataInId == dataInId, cancellationToken);

        if (dataIn.Status == OperationStatus.TODO)
        {
            dataIn.MarkAsDoing();
        }

        if (hasSumm && hasSubtraction && dataIn.Status != OperationStatus.DONE)
        {
            dataIn.MarkAsDone();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkProcessingStartedAsync(DataIn dataIn, CancellationToken cancellationToken = default)
    {
        if (dataIn.Status == OperationStatus.DONE)
        {
            return;
        }

        if (dataIn.Status == OperationStatus.TODO)
        {
            dataIn.MarkAsDoing();
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

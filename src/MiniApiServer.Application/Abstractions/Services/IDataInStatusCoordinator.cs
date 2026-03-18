using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Abstractions.Services;

public interface IDataInStatusCoordinator
{
    Task MarkProcessingStartedAsync(DataIn dataIn, CancellationToken cancellationToken = default);

    Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default);
}

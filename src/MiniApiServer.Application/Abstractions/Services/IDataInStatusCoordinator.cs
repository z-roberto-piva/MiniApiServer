using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Abstractions.Services;

/// <summary>
/// Centralizes the status transitions of a <see cref="DataIn"/> record during job execution.
/// </summary>
public interface IDataInStatusCoordinator
{
    /// <summary>
    /// Marks processing as started for the specified input.
    /// </summary>
    Task MarkProcessingStartedAsync(DataIn dataIn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks one operation as completed and finalizes the input when all work is done.
    /// </summary>
    Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default);
}

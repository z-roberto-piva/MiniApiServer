using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists results produced by sum operations.
/// </summary>
public interface IDataSummRepository
{
    /// <summary>
    /// Stores a sum result.
    /// </summary>
    Task AddAsync(DataSumm dataSumm, CancellationToken cancellationToken = default);
}

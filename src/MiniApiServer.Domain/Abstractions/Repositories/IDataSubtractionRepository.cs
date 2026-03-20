using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

/// <summary>
/// Persists results produced by subtraction operations.
/// </summary>
public interface IDataSubtractionRepository
{
    /// <summary>
    /// Stores a subtraction result.
    /// </summary>
    Task AddAsync(DataSubtraction dataSubtraction, CancellationToken cancellationToken = default);
}

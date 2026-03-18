using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IDataSubtractionRepository
{
    Task AddAsync(DataSubtraction dataSubtraction, CancellationToken cancellationToken = default);
}

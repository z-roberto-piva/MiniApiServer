using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IStatRepository
{
    Task AddAsync(Stat stat, CancellationToken cancellationToken = default);
}

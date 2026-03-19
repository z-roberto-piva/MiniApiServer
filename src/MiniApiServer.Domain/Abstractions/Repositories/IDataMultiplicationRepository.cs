using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IDataMultiplicationRepository
{
    Task AddAsync(DataMultiplication dataMultiplication, CancellationToken cancellationToken = default);
}

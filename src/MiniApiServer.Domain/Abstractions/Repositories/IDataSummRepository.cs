using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IDataSummRepository
{
    Task AddAsync(DataSumm dataSumm, CancellationToken cancellationToken = default);
}

using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Domain.Abstractions.Repositories;

public interface IDataDivisionRepository
{
    Task AddAsync(DataDivision dataDivision, CancellationToken cancellationToken = default);
}

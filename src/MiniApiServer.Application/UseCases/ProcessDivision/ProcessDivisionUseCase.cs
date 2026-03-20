using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.ProcessDivision;

/// <summary>
/// Executes the division operation for a persisted input and stores the result.
/// </summary>
public sealed class ProcessDivisionUseCase(
    IDataInRepository dataInRepository,
    IDataDivisionRepository dataDivisionRepository,
    IDataInStatusCoordinator dataInStatusCoordinator)
{
    /// <summary>
    /// Processes the input identified by the command and persists the division result.
    /// </summary>
    public async Task<ProcessDivisionResult> ExecuteAsync(ProcessDivisionCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = await dataInRepository.GetByIdAsync(command.DataInId, cancellationToken);

        if (dataIn is null)
        {
            throw new AppLayerException($"Data input with id '{command.DataInId}' was not found.");
        }

        await dataInStatusCoordinator.MarkProcessingStartedAsync(dataIn, cancellationToken);

        var result = dataIn.DataB == 0 ? 0 : dataIn.DataA / dataIn.DataB;
        var division = DataDivision.Create(dataIn.Id, dataIn.Description, result);

        await dataDivisionRepository.AddAsync(division, cancellationToken);
        await dataInStatusCoordinator.MarkOperationCompletedAsync(dataIn.Id, cancellationToken);

        return new ProcessDivisionResult(dataIn.Id, division.Result);
    }
}

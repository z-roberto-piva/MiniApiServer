using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.ProcessSum;

/// <summary>
/// Executes the sum operation for a persisted input and stores the result.
/// </summary>
public sealed class ProcessSumUseCase(
    IDataInRepository dataInRepository,
    IDataSummRepository dataSummRepository,
    IDataInStatusCoordinator dataInStatusCoordinator)
{
    /// <summary>
    /// Processes the input identified by the command and persists the sum result.
    /// </summary>
    public async Task<ProcessSumResult> ExecuteAsync(ProcessSumCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = await dataInRepository.GetByIdAsync(command.DataInId, cancellationToken);

        if (dataIn is null)
        {
            throw new AppLayerException($"Data input with id '{command.DataInId}' was not found.");
        }

        await dataInStatusCoordinator.MarkProcessingStartedAsync(dataIn, cancellationToken);

        var summ = DataSumm.Create(dataIn.Id, dataIn.Description, dataIn.DataA + dataIn.DataB);

        await dataSummRepository.AddAsync(summ, cancellationToken);
        await dataInStatusCoordinator.MarkOperationCompletedAsync(dataIn.Id, cancellationToken);

        return new ProcessSumResult(dataIn.Id, summ.Result);
    }
}

using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.ProcessSubtraction;

/// <summary>
/// Executes the subtraction operation for a persisted input and stores the result.
/// </summary>
public sealed class ProcessSubtractionUseCase(
    IDataInRepository dataInRepository,
    IDataSubtractionRepository dataSubtractionRepository,
    IDataInStatusCoordinator dataInStatusCoordinator)
{
    /// <summary>
    /// Processes the input identified by the command and persists the subtraction result.
    /// </summary>
    public async Task<ProcessSubtractionResult> ExecuteAsync(ProcessSubtractionCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = await dataInRepository.GetByIdAsync(command.DataInId, cancellationToken);

        if (dataIn is null)
        {
            throw new AppLayerException($"Data input with id '{command.DataInId}' was not found.");
        }

        await dataInStatusCoordinator.MarkProcessingStartedAsync(dataIn, cancellationToken);

        var subtraction = DataSubtraction.Create(dataIn.Id, dataIn.Description, dataIn.DataA - dataIn.DataB);

        await dataSubtractionRepository.AddAsync(subtraction, cancellationToken);
        await dataInStatusCoordinator.MarkOperationCompletedAsync(dataIn.Id, cancellationToken);

        return new ProcessSubtractionResult(dataIn.Id, subtraction.Result);
    }
}

using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.ProcessMultiplication;

public sealed class ProcessMultiplicationUseCase(
    IDataInRepository dataInRepository,
    IDataMultiplicationRepository dataMultiplicationRepository,
    IDataInStatusCoordinator dataInStatusCoordinator)
{
    public async Task<ProcessMultiplicationResult> ExecuteAsync(ProcessMultiplicationCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = await dataInRepository.GetByIdAsync(command.DataInId, cancellationToken);

        if (dataIn is null)
        {
            throw new AppLayerException($"Data input with id '{command.DataInId}' was not found.");
        }

        await dataInStatusCoordinator.MarkProcessingStartedAsync(dataIn, cancellationToken);

        var multiplication = DataMultiplication.Create(dataIn.Id, dataIn.Description, dataIn.DataA * dataIn.DataB);

        await dataMultiplicationRepository.AddAsync(multiplication, cancellationToken);
        await dataInStatusCoordinator.MarkOperationCompletedAsync(dataIn.Id, cancellationToken);

        return new ProcessMultiplicationResult(dataIn.Id, multiplication.Result);
    }
}

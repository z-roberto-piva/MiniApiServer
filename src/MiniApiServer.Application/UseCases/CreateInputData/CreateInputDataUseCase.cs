using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.CreateInputData;

/// <summary>
/// Creates a new input row and schedules the background jobs that will process it.
/// </summary>
public sealed class CreateInputDataUseCase(
    IDataInRepository dataInRepository,
    IBackgroundJobScheduler backgroundJobScheduler,
    IBackgroundJobCategoryResolver backgroundJobCategoryResolver)
{
    /// <summary>
    /// Persists the input and enqueues all configured processing jobs.
    /// </summary>
    public async Task<CreateInputDataResult> ExecuteAsync(CreateInputDataCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = DataIn.Create(command.Description, command.DataA, command.DataB);
        var multiplicationCategory = backgroundJobCategoryResolver.ResolveCategory(BackgroundJobType.ProcessMultiplication);
        var divisionCategory = backgroundJobCategoryResolver.ResolveCategory(BackgroundJobType.ProcessDivision);
        var sumCategory = backgroundJobCategoryResolver.ResolveCategory(BackgroundJobType.ProcessSum);
        var subtractionCategory = backgroundJobCategoryResolver.ResolveCategory(BackgroundJobType.ProcessSubtraction);

        await dataInRepository.AddAsync(dataIn, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessMultiplicationAsync(dataIn.Id, multiplicationCategory, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessDivisionAsync(dataIn.Id, divisionCategory, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessSumAsync(dataIn.Id, sumCategory, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessSubtractionAsync(dataIn.Id, subtractionCategory, cancellationToken);

        return new CreateInputDataResult(
            dataIn.Id,
            dataIn.Description,
            dataIn.DataA,
            dataIn.DataB,
            dataIn.Status);
    }
}

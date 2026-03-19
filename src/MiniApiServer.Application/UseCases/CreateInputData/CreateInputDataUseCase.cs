using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.UseCases.CreateInputData;

public sealed class CreateInputDataUseCase(IDataInRepository dataInRepository, IBackgroundJobScheduler backgroundJobScheduler)
{
    public async Task<CreateInputDataResult> ExecuteAsync(CreateInputDataCommand command, CancellationToken cancellationToken = default)
    {
        var dataIn = DataIn.Create(command.Description, command.DataA, command.DataB);

        await dataInRepository.AddAsync(dataIn, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessMultiplicationAsync(dataIn.Id, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessDivisionAsync(dataIn.Id, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessSumAsync(dataIn.Id, cancellationToken);
        await backgroundJobScheduler.EnqueueProcessSubtractionAsync(dataIn.Id, cancellationToken);

        return new CreateInputDataResult(
            dataIn.Id,
            dataIn.Description,
            dataIn.DataA,
            dataIn.DataB,
            dataIn.Status);
    }
}

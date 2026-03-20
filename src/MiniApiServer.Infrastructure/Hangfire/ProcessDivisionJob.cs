using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessDivision;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessDivisionJob(
    ProcessDivisionUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessDivisionJob> logger)
{
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting division job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("division", dataInId);
        await useCase.ExecuteAsync(new ProcessDivisionCommand(dataInId));
        logger.LogInformation("Completed division job for data input {DataInId}.", dataInId);
    }
}

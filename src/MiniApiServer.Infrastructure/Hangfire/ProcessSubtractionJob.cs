using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSubtraction;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessSubtractionJob(
    ProcessSubtractionUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessSubtractionJob> logger)
{
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting subtraction job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("subtraction", dataInId);
        await useCase.ExecuteAsync(new ProcessSubtractionCommand(dataInId));
        logger.LogInformation("Completed subtraction job for data input {DataInId}.", dataInId);
    }
}

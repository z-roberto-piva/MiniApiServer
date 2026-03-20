using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessMultiplication;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessMultiplicationJob(
    ProcessMultiplicationUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessMultiplicationJob> logger)
{
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting multiplication job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("multiplication", dataInId);
        await useCase.ExecuteAsync(new ProcessMultiplicationCommand(dataInId));
        logger.LogInformation("Completed multiplication job for data input {DataInId}.", dataInId);
    }
}

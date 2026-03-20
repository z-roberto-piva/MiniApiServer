using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSum;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Hangfire wrapper that executes the sum use case.
/// </summary>
public sealed class ProcessSumJob(
    ProcessSumUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessSumJob> logger)
{
    /// <summary>
    /// Processes the sum for the specified input.
    /// </summary>
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting sum job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("sum", dataInId);
        await useCase.ExecuteAsync(new ProcessSumCommand(dataInId));
        logger.LogInformation("Completed sum job for data input {DataInId}.", dataInId);
    }
}

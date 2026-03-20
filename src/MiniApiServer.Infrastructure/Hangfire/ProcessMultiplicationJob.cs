using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessMultiplication;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Hangfire wrapper that executes the multiplication use case.
/// </summary>
public sealed class ProcessMultiplicationJob(
    ProcessMultiplicationUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessMultiplicationJob> logger)
{
    /// <summary>
    /// Processes the multiplication for the specified input.
    /// </summary>
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting multiplication job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("multiplication", dataInId);
        await useCase.ExecuteAsync(new ProcessMultiplicationCommand(dataInId));
        logger.LogInformation("Completed multiplication job for data input {DataInId}.", dataInId);
    }
}

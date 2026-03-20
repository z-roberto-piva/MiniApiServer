using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSubtraction;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Hangfire wrapper that executes the subtraction use case.
/// </summary>
public sealed class ProcessSubtractionJob(
    ProcessSubtractionUseCase useCase,
    IJobExecutionDelaySimulator delaySimulator,
    ILogger<ProcessSubtractionJob> logger)
{
    /// <summary>
    /// Processes the subtraction for the specified input.
    /// </summary>
    public async Task ExecuteAsync(Guid dataInId)
    {
        logger.LogInformation("Starting subtraction job for data input {DataInId}.", dataInId);
        await delaySimulator.DelayAsync("subtraction", dataInId);
        await useCase.ExecuteAsync(new ProcessSubtractionCommand(dataInId));
        logger.LogInformation("Completed subtraction job for data input {DataInId}.", dataInId);
    }
}

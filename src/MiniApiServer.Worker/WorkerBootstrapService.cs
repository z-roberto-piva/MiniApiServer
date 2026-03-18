using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MiniApiServer.Worker;

public sealed class WorkerBootstrapService(ILogger<WorkerBootstrapService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("MiniApiServer.Worker avviato. L'esecuzione dei job Hangfire verra configurata nelle fasi successive.");
        return Task.CompletedTask;
    }
}

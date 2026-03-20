namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Configures the Hangfire queues used by the worker.
/// </summary>
public sealed class HangfireQueueOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Hangfire:Queues";

    /// <summary>
    /// Explicit queue order for the worker server.
    /// </summary>
    public string[] WorkerQueues { get; init; } = ["high", "standard", "low"];

    /// <summary>
    /// Queue name for high-priority jobs.
    /// </summary>
    public string HighPriority { get; init; } = "high";

    /// <summary>
    /// Queue name for standard-priority jobs.
    /// </summary>
    public string StandardPriority { get; init; } = "standard";

    /// <summary>
    /// Queue name for low-priority jobs.
    /// </summary>
    public string LowPriority { get; init; } = "low";
}

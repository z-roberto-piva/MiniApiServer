namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class HangfireQueueOptions
{
    public const string SectionName = "Hangfire:Queues";

    public string[] WorkerQueues { get; init; } = ["high", "standard", "low"];

    public string HighPriority { get; init; } = "high";

    public string StandardPriority { get; init; } = "standard";

    public string LowPriority { get; init; } = "low";
}

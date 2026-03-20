namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class JobExecutionDelayOptions
{
    public const string SectionName = "Hangfire:JobExecutionDelay";

    public static IReadOnlyList<int> DefaultAllowedDurationsInSeconds { get; } = [5, 10, 15, 20, 30, 40, 50, 60];

    public bool Enabled { get; init; }

    public int[]? AllowedDurationsInSeconds { get; init; }
}

namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Configures the optional simulated delay applied before executing background jobs.
/// </summary>
public sealed class JobExecutionDelayOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Hangfire:JobExecutionDelay";

    /// <summary>
    /// Default set of durations used when the configuration does not provide a custom list.
    /// </summary>
    public static IReadOnlyList<int> DefaultAllowedDurationsInSeconds { get; } = [5, 10, 15, 20, 30, 40, 50, 60];

    /// <summary>
    /// Enables or disables delay simulation.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Candidate delay durations expressed in seconds.
    /// </summary>
    public int[]? AllowedDurationsInSeconds { get; init; }
}

namespace MiniApiServer.Application.Abstractions.Jobs;

/// <summary>
/// Logical priority bucket assigned to background jobs before queue resolution.
/// </summary>
public enum BackgroundJobCategory
{
    /// <summary>
    /// Highest processing priority.
    /// </summary>
    HighPriority = 1,

    /// <summary>
    /// Default processing priority.
    /// </summary>
    StandardPriority = 2,

    /// <summary>
    /// Lowest processing priority.
    /// </summary>
    LowPriority = 3
}

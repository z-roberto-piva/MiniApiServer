namespace MiniApiServer.Application.Abstractions.Jobs;

/// <summary>
/// Identifies the supported background jobs in the application layer.
/// </summary>
public enum BackgroundJobType
{
    /// <summary>
    /// Addition job.
    /// </summary>
    ProcessSum = 1,

    /// <summary>
    /// Subtraction job.
    /// </summary>
    ProcessSubtraction = 2,

    /// <summary>
    /// Multiplication job.
    /// </summary>
    ProcessMultiplication = 3,

    /// <summary>
    /// Division job.
    /// </summary>
    ProcessDivision = 4,

    /// <summary>
    /// Daily statistics aggregation job.
    /// </summary>
    GenerateDailyStats = 5
}

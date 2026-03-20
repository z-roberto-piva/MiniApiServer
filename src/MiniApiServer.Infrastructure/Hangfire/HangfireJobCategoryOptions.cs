namespace MiniApiServer.Infrastructure.Hangfire;

/// <summary>
/// Configures the category assigned to each logical background job.
/// </summary>
public sealed class HangfireJobCategoryOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "Hangfire:JobCategories";

    /// <summary>
    /// Category assigned to sum jobs.
    /// </summary>
    public string ProcessSum { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    /// <summary>
    /// Category assigned to subtraction jobs.
    /// </summary>
    public string ProcessSubtraction { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    /// <summary>
    /// Category assigned to multiplication jobs.
    /// </summary>
    public string ProcessMultiplication { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    /// <summary>
    /// Category assigned to division jobs.
    /// </summary>
    public string ProcessDivision { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    /// <summary>
    /// Category assigned to the recurring daily statistics job.
    /// </summary>
    public string GenerateDailyStats { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.LowPriority);
}

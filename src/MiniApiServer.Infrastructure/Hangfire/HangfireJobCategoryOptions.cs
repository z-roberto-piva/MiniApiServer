namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class HangfireJobCategoryOptions
{
    public const string SectionName = "Hangfire:JobCategories";

    public string ProcessSum { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    public string ProcessSubtraction { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    public string ProcessMultiplication { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    public string ProcessDivision { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority);

    public string GenerateDailyStats { get; init; } = nameof(Application.Abstractions.Jobs.BackgroundJobCategory.LowPriority);
}

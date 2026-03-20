using Microsoft.Extensions.Options;
using MiniApiServer.Application.Abstractions.Jobs;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ConfiguredBackgroundJobCategoryResolver(IOptions<HangfireJobCategoryOptions> options) : IBackgroundJobCategoryResolver
{
    public BackgroundJobCategory ResolveCategory(BackgroundJobType jobType)
    {
        var configuredValue = jobType switch
        {
            BackgroundJobType.ProcessSum => options.Value.ProcessSum,
            BackgroundJobType.ProcessSubtraction => options.Value.ProcessSubtraction,
            BackgroundJobType.ProcessMultiplication => options.Value.ProcessMultiplication,
            BackgroundJobType.ProcessDivision => options.Value.ProcessDivision,
            BackgroundJobType.GenerateDailyStats => options.Value.GenerateDailyStats,
            _ => nameof(BackgroundJobCategory.StandardPriority)
        };

        return Enum.TryParse<BackgroundJobCategory>(configuredValue, ignoreCase: true, out var category)
            ? category
            : BackgroundJobCategory.StandardPriority;
    }
}

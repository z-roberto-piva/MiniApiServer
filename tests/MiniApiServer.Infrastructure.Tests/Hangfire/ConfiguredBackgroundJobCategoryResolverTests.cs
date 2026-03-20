using Microsoft.Extensions.Options;
using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Infrastructure.Hangfire;

namespace MiniApiServer.Infrastructure.Tests.Hangfire;

public sealed class ConfiguredBackgroundJobCategoryResolverTests
{
    [Fact]
    public void ResolveCategory_ShouldReadConfiguredCategoryNames()
    {
        var options = Options.Create(new HangfireJobCategoryOptions
        {
            ProcessSum = "HighPriority",
            ProcessSubtraction = "StandardPriority",
            ProcessMultiplication = "LowPriority",
            ProcessDivision = "StandardPriority",
            GenerateDailyStats = "LowPriority"
        });
        var resolver = new ConfiguredBackgroundJobCategoryResolver(options);

        Assert.Equal(BackgroundJobCategory.HighPriority, resolver.ResolveCategory(BackgroundJobType.ProcessSum));
        Assert.Equal(BackgroundJobCategory.StandardPriority, resolver.ResolveCategory(BackgroundJobType.ProcessSubtraction));
        Assert.Equal(BackgroundJobCategory.LowPriority, resolver.ResolveCategory(BackgroundJobType.ProcessMultiplication));
        Assert.Equal(BackgroundJobCategory.StandardPriority, resolver.ResolveCategory(BackgroundJobType.ProcessDivision));
        Assert.Equal(BackgroundJobCategory.LowPriority, resolver.ResolveCategory(BackgroundJobType.GenerateDailyStats));
    }

    [Fact]
    public void ResolveCategory_ShouldFallbackToStandardWhenValueIsInvalid()
    {
        var options = Options.Create(new HangfireJobCategoryOptions
        {
            ProcessSum = "unsupported"
        });
        var resolver = new ConfiguredBackgroundJobCategoryResolver(options);

        Assert.Equal(BackgroundJobCategory.StandardPriority, resolver.ResolveCategory(BackgroundJobType.ProcessSum));
    }
}

using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.UseCases.CreateInputData;
using MiniApiServer.Application.UseCases.GenerateDailyStats;
using MiniApiServer.Application.UseCases.ProcessDivision;
using MiniApiServer.Application.UseCases.ProcessMultiplication;
using MiniApiServer.Application.UseCases.ProcessSubtraction;
using MiniApiServer.Application.UseCases.ProcessSum;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Infrastructure.Hangfire;
using MiniApiServer.Infrastructure.Persistence;
using MiniApiServer.Infrastructure.Persistence.Queries;
using MiniApiServer.Infrastructure.Persistence.Repositories;
using MiniApiServer.Infrastructure.Persistence.Services;

namespace MiniApiServer.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var appConnectionString = configuration.GetConnectionString("AppPostgres")
            ?? throw new InvalidOperationException("Connection string 'AppPostgres' is not configured.");

        var hangfireConnectionString = configuration.GetConnectionString("HangfirePostgres")
            ?? throw new InvalidOperationException("Connection string 'HangfirePostgres' is not configured.");

        services.AddDbContext<MiniApiServerDbContext>(options => options.UseNpgsql(
            appConnectionString,
            npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", MiniApiServerDbContext.Schema)));

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(storage => storage.UseNpgsqlConnection(hangfireConnectionString)));

        var delaySection = configuration.GetSection(JobExecutionDelayOptions.SectionName);
        var delayOptions = new JobExecutionDelayOptions
        {
            Enabled = bool.TryParse(delaySection["Enabled"], out var enabled) && enabled,
            AllowedDurationsInSeconds = delaySection.GetSection("AllowedDurationsInSeconds")
                .GetChildren()
                .Select(section => int.TryParse(section.Value, out var seconds) ? seconds : (int?)null)
                .Where(seconds => seconds.HasValue)
                .Select(seconds => seconds!.Value)
                .ToArray()
        };

        services.AddSingleton<IOptions<JobExecutionDelayOptions>>(Options.Create(delayOptions));

        var queueSection = configuration.GetSection(HangfireQueueOptions.SectionName);
        var queueOptions = new HangfireQueueOptions
        {
            WorkerQueues = queueSection.GetSection("WorkerQueues")
                .GetChildren()
                .Select(section => section.Value)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Cast<string>()
                .ToArray(),
            HighPriority = queueSection["HighPriority"] ?? "high",
            StandardPriority = queueSection["StandardPriority"] ?? "standard",
            LowPriority = queueSection["LowPriority"] ?? "low"
        };

        services.AddSingleton<IOptions<HangfireQueueOptions>>(Options.Create(queueOptions));

        var categorySection = configuration.GetSection(HangfireJobCategoryOptions.SectionName);
        var categoryOptions = new HangfireJobCategoryOptions
        {
            ProcessSum = categorySection["ProcessSum"] ?? nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority),
            ProcessSubtraction = categorySection["ProcessSubtraction"] ?? nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority),
            ProcessMultiplication = categorySection["ProcessMultiplication"] ?? nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority),
            ProcessDivision = categorySection["ProcessDivision"] ?? nameof(Application.Abstractions.Jobs.BackgroundJobCategory.HighPriority),
            GenerateDailyStats = categorySection["GenerateDailyStats"] ?? nameof(Application.Abstractions.Jobs.BackgroundJobCategory.LowPriority)
        };

        services.AddSingleton<IOptions<HangfireJobCategoryOptions>>(Options.Create(categoryOptions));

        services.AddScoped<IDataInRepository, DataInRepository>();
        services.AddScoped<IDataDivisionRepository, DataDivisionRepository>();
        services.AddScoped<IDataMultiplicationRepository, DataMultiplicationRepository>();
        services.AddScoped<IDataSummRepository, DataSummRepository>();
        services.AddScoped<IDataSubtractionRepository, DataSubtractionRepository>();
        services.AddScoped<IStatRepository, StatRepository>();
        services.AddScoped<IDailyOperationsSummaryReader, DailyOperationsSummaryReader>();
        services.AddScoped<IDataInStatusCoordinator, DataInStatusCoordinator>();
        services.AddSingleton<IBackgroundJobCategoryResolver, ConfiguredBackgroundJobCategoryResolver>();
        services.AddScoped<IBackgroundJobScheduler, HangfireBackgroundJobScheduler>();
        services.AddSingleton<IBackgroundJobQueueResolver, BackgroundJobQueueResolver>();
        services.AddSingleton<IRandomDelaySelector, RandomDelaySelector>();
        services.AddSingleton<IJobDelayAwaiter, TaskDelayAwaiter>();
        services.AddSingleton<IJobExecutionDelaySimulator, JobExecutionDelaySimulator>();

        services.AddScoped<CreateInputDataUseCase>();
        services.AddScoped<ProcessDivisionUseCase>();
        services.AddScoped<ProcessMultiplicationUseCase>();
        services.AddScoped<ProcessSumUseCase>();
        services.AddScoped<ProcessSubtractionUseCase>();
        services.AddScoped<GenerateDailyStatsUseCase>();

        services.AddScoped<ProcessDivisionJob>();
        services.AddScoped<ProcessMultiplicationJob>();
        services.AddScoped<ProcessSumJob>();
        services.AddScoped<ProcessSubtractionJob>();
        services.AddScoped<GenerateDailyStatsRecurringJob>();

        return services;
    }

    public static IServiceCollection AddHangfireWorker(this IServiceCollection services, IConfiguration configuration)
    {
        var queueSection = configuration.GetSection(HangfireQueueOptions.SectionName);
        var workerQueues = queueSection.GetSection("WorkerQueues")
            .GetChildren()
            .Select(section => section.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Cast<string>()
            .ToArray();

        services.AddHangfireServer(options =>
        {
            options.Queues = workerQueues.Length > 0 ? workerQueues : ["high", "standard", "low"];
        });
        return services;
    }
}

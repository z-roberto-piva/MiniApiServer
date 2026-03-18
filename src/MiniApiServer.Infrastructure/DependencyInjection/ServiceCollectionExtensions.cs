using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.UseCases.CreateInputData;
using MiniApiServer.Application.UseCases.GenerateDailyStats;
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

        services.AddDbContext<MiniApiServerDbContext>(options => options.UseNpgsql(appConnectionString));

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(storage => storage.UseNpgsqlConnection(hangfireConnectionString)));

        services.AddScoped<IDataInRepository, DataInRepository>();
        services.AddScoped<IDataSummRepository, DataSummRepository>();
        services.AddScoped<IDataSubtractionRepository, DataSubtractionRepository>();
        services.AddScoped<IStatRepository, StatRepository>();
        services.AddScoped<IDailyOperationsSummaryReader, DailyOperationsSummaryReader>();
        services.AddScoped<IDataInStatusCoordinator, DataInStatusCoordinator>();
        services.AddScoped<IBackgroundJobScheduler, HangfireBackgroundJobScheduler>();

        services.AddScoped<CreateInputDataUseCase>();
        services.AddScoped<ProcessSumUseCase>();
        services.AddScoped<ProcessSubtractionUseCase>();
        services.AddScoped<GenerateDailyStatsUseCase>();

        services.AddScoped<ProcessSumJob>();
        services.AddScoped<ProcessSubtractionJob>();
        services.AddScoped<GenerateDailyStatsRecurringJob>();

        return services;
    }

    public static IServiceCollection AddHangfireWorker(this IServiceCollection services)
    {
        services.AddHangfireServer();
        return services;
    }
}

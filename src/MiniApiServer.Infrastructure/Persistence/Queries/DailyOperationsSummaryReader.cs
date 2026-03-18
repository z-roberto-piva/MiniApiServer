using Microsoft.EntityFrameworkCore;
using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Contracts;

namespace MiniApiServer.Infrastructure.Persistence.Queries;

public sealed class DailyOperationsSummaryReader(MiniApiServerDbContext dbContext) : IDailyOperationsSummaryReader
{
    public async Task<DailyOperationsSummary> GetForDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        var startUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var endUtc = startUtc.AddDays(1);

        var numberOfOperations = await dbContext.DataIns
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .CountAsync(cancellationToken);

        var totalOfSums = await dbContext.DataSumms
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .Select(entity => (int?)entity.Result)
            .SumAsync(cancellationToken) ?? 0;

        var totalOfSubtractions = await dbContext.DataSubtractions
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .Select(entity => (int?)entity.Result)
            .SumAsync(cancellationToken) ?? 0;

        return new DailyOperationsSummary(date, numberOfOperations, totalOfSums, totalOfSubtractions);
    }
}

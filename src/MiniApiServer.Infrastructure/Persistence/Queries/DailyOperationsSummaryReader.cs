using Microsoft.EntityFrameworkCore;
using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Contracts;

namespace MiniApiServer.Infrastructure.Persistence.Queries;

/// <summary>
/// Reads the daily aggregation required to build <see cref="Domain.Entities.Stat"/> rows.
/// </summary>
public sealed class DailyOperationsSummaryReader(MiniApiServerDbContext dbContext) : IDailyOperationsSummaryReader
{
    /// <summary>
    /// Returns the aggregated totals for the requested UTC day.
    /// </summary>
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
            .CountAsync(cancellationToken);

        var totalOfSubtractions = await dbContext.DataSubtractions
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .CountAsync(cancellationToken);

        var totalOfMultiplications = await dbContext.DataMultiplications
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .CountAsync(cancellationToken);

        var totalOfDivisions = await dbContext.DataDivisions
            .Where(entity => EF.Property<DateTime>(entity, "created_at_utc") >= startUtc
                && EF.Property<DateTime>(entity, "created_at_utc") < endUtc)
            .CountAsync(cancellationToken);

        return new DailyOperationsSummary(
            date,
            numberOfOperations,
            totalOfSums,
            totalOfSubtractions,
            totalOfMultiplications,
            totalOfDivisions);
    }
}

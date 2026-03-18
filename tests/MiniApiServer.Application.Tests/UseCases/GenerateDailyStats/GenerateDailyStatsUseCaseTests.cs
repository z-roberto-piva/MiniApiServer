using MiniApiServer.Application.Abstractions.Queries;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.GenerateDailyStats;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Tests.UseCases.GenerateDailyStats;

public sealed class GenerateDailyStatsUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateDailyStatFromSummaryReader()
    {
        var reader = new FixedDailyOperationsSummaryReader(new DailyOperationsSummary(new DateOnly(2026, 3, 18), 2, 30, 5));
        var repository = new RecordingStatRepository();
        var useCase = new GenerateDailyStatsUseCase(reader, repository);

        var result = await useCase.ExecuteAsync(new GenerateDailyStatsCommand(new DateOnly(2026, 3, 18)));

        Assert.Equal(2, result.NumberOfOperations);
        Assert.Equal(30, result.TotalOfSums);
        Assert.Equal(5, result.TotalOfSubtractions);
        Assert.Single(repository.Items);
    }

    private sealed class FixedDailyOperationsSummaryReader(DailyOperationsSummary summary) : IDailyOperationsSummaryReader
    {
        public Task<DailyOperationsSummary> GetForDateAsync(DateOnly date, CancellationToken cancellationToken = default)
            => Task.FromResult(summary);
    }

    private sealed class RecordingStatRepository : IStatRepository
    {
        public List<Stat> Items { get; } = [];

        public Task AddAsync(Stat stat, CancellationToken cancellationToken = default)
        {
            Items.Add(stat);
            return Task.CompletedTask;
        }
    }
}

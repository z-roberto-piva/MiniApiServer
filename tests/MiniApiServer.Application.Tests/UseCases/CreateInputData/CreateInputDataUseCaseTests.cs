using MiniApiServer.Application.Abstractions.Jobs;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.CreateInputData;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;
using MiniApiServer.Domain.Enums;

namespace MiniApiServer.Application.Tests.UseCases.CreateInputData;

public sealed class CreateInputDataUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldPersistInputAndEnqueueAllJobs()
    {
        var repository = new InMemoryDataInRepository();
        var scheduler = new RecordingBackgroundJobScheduler();
        var categoryResolver = new StubBackgroundJobCategoryResolver();
        var useCase = new CreateInputDataUseCase(repository, scheduler, categoryResolver);

        var result = await useCase.ExecuteAsync(new CreateInputDataCommand("input", 5, 2));

        Assert.NotEqual(Guid.Empty, result.DataInId);
        Assert.Equal(OperationStatus.TODO, result.Status);
        Assert.Single(repository.Items);
        Assert.Equal(result.DataInId, scheduler.MultiplicationJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.DivisionJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.SumJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.SubtractionJobIds.Single());
        Assert.Equal(BackgroundJobCategory.StandardPriority, scheduler.MultiplicationCategories.Single());
        Assert.Equal(BackgroundJobCategory.LowPriority, scheduler.DivisionCategories.Single());
        Assert.Equal(BackgroundJobCategory.HighPriority, scheduler.SumCategories.Single());
        Assert.Equal(BackgroundJobCategory.StandardPriority, scheduler.SubtractionCategories.Single());
    }

    private sealed class InMemoryDataInRepository : IDataInRepository
    {
        public List<DataIn> Items { get; } = [];

        public Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default)
        {
            Items.Add(dataIn);
            return Task.CompletedTask;
        }

        public Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Items.SingleOrDefault(item => item.Id == id));

        public Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class RecordingBackgroundJobScheduler : IBackgroundJobScheduler
    {
        public List<BackgroundJobCategory> DivisionCategories { get; } = [];

        public List<Guid> DivisionJobIds { get; } = [];

        public List<BackgroundJobCategory> MultiplicationCategories { get; } = [];

        public List<Guid> MultiplicationJobIds { get; } = [];

        public List<BackgroundJobCategory> SumCategories { get; } = [];

        public List<Guid> SumJobIds { get; } = [];

        public List<BackgroundJobCategory> SubtractionCategories { get; } = [];

        public List<Guid> SubtractionJobIds { get; } = [];

        public Task EnqueueProcessDivisionAsync(Guid dataInId, BackgroundJobCategory category = BackgroundJobCategory.HighPriority, CancellationToken cancellationToken = default)
        {
            DivisionJobIds.Add(dataInId);
            DivisionCategories.Add(category);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessMultiplicationAsync(Guid dataInId, BackgroundJobCategory category = BackgroundJobCategory.HighPriority, CancellationToken cancellationToken = default)
        {
            MultiplicationJobIds.Add(dataInId);
            MultiplicationCategories.Add(category);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessSubtractionAsync(Guid dataInId, BackgroundJobCategory category = BackgroundJobCategory.HighPriority, CancellationToken cancellationToken = default)
        {
            SubtractionJobIds.Add(dataInId);
            SubtractionCategories.Add(category);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessSumAsync(Guid dataInId, BackgroundJobCategory category = BackgroundJobCategory.HighPriority, CancellationToken cancellationToken = default)
        {
            SumJobIds.Add(dataInId);
            SumCategories.Add(category);
            return Task.CompletedTask;
        }
    }

    private sealed class StubBackgroundJobCategoryResolver : IBackgroundJobCategoryResolver
    {
        public BackgroundJobCategory ResolveCategory(BackgroundJobType jobType)
            => jobType switch
            {
                BackgroundJobType.ProcessSum => BackgroundJobCategory.HighPriority,
                BackgroundJobType.ProcessSubtraction => BackgroundJobCategory.StandardPriority,
                BackgroundJobType.ProcessMultiplication => BackgroundJobCategory.StandardPriority,
                BackgroundJobType.ProcessDivision => BackgroundJobCategory.LowPriority,
                _ => BackgroundJobCategory.StandardPriority
            };
    }
}

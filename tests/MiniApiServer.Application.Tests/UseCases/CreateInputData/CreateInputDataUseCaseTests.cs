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
        var useCase = new CreateInputDataUseCase(repository, scheduler);

        var result = await useCase.ExecuteAsync(new CreateInputDataCommand("input", 5, 2));

        Assert.NotEqual(Guid.Empty, result.DataInId);
        Assert.Equal(OperationStatus.TODO, result.Status);
        Assert.Single(repository.Items);
        Assert.Equal(result.DataInId, scheduler.MultiplicationJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.DivisionJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.SumJobIds.Single());
        Assert.Equal(result.DataInId, scheduler.SubtractionJobIds.Single());
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
        public List<Guid> DivisionJobIds { get; } = [];

        public List<Guid> MultiplicationJobIds { get; } = [];

        public List<Guid> SumJobIds { get; } = [];

        public List<Guid> SubtractionJobIds { get; } = [];

        public Task EnqueueProcessDivisionAsync(Guid dataInId, CancellationToken cancellationToken = default)
        {
            DivisionJobIds.Add(dataInId);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessMultiplicationAsync(Guid dataInId, CancellationToken cancellationToken = default)
        {
            MultiplicationJobIds.Add(dataInId);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessSubtractionAsync(Guid dataInId, CancellationToken cancellationToken = default)
        {
            SubtractionJobIds.Add(dataInId);
            return Task.CompletedTask;
        }

        public Task EnqueueProcessSumAsync(Guid dataInId, CancellationToken cancellationToken = default)
        {
            SumJobIds.Add(dataInId);
            return Task.CompletedTask;
        }
    }
}

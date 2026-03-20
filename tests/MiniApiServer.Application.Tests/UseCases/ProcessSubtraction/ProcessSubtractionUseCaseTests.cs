using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSubtraction;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Tests.UseCases.ProcessSubtraction;

public sealed class ProcessSubtractionUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateSubtractionAndNotifyCoordinator()
    {
        var dataIn = DataIn.Create("input", 10, 3);
        var dataInRepository = new SingleDataInRepository(dataIn);
        var subtractionRepository = new RecordingDataSubtractionRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessSubtractionUseCase(dataInRepository, subtractionRepository, coordinator);

        var result = await useCase.ExecuteAsync(new ProcessSubtractionCommand(dataIn.Id));

        Assert.Equal(7, result.Result);
        Assert.Single(subtractionRepository.Items);
        Assert.Equal(dataIn.Id, coordinator.StartedIds.Single());
        Assert.Equal(dataIn.Id, coordinator.CompletedIds.Single());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowWhenInputDoesNotExist()
    {
        var dataInRepository = new SingleDataInRepository(null);
        var subtractionRepository = new RecordingDataSubtractionRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessSubtractionUseCase(dataInRepository, subtractionRepository, coordinator);

        var action = () => useCase.ExecuteAsync(new ProcessSubtractionCommand(Guid.NewGuid()));

        var exception = await Assert.ThrowsAsync<AppLayerException>(action);

        Assert.Contains("was not found", exception.Message, StringComparison.Ordinal);
    }

    private sealed class SingleDataInRepository(DataIn? item) : IDataInRepository
    {
        public Task AddAsync(DataIn dataIn, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task<DataIn?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(item is not null && item.Id == id ? item : null);

        public Task UpdateAsync(DataIn dataIn, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class RecordingDataSubtractionRepository : IDataSubtractionRepository
    {
        public List<DataSubtraction> Items { get; } = [];

        public Task AddAsync(DataSubtraction dataSubtraction, CancellationToken cancellationToken = default)
        {
            Items.Add(dataSubtraction);
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingDataInStatusCoordinator : IDataInStatusCoordinator
    {
        public List<Guid> StartedIds { get; } = [];

        public List<Guid> CompletedIds { get; } = [];

        public Task MarkOperationCompletedAsync(Guid dataInId, CancellationToken cancellationToken = default)
        {
            CompletedIds.Add(dataInId);
            return Task.CompletedTask;
        }

        public Task MarkProcessingStartedAsync(DataIn dataIn, CancellationToken cancellationToken = default)
        {
            StartedIds.Add(dataIn.Id);
            return Task.CompletedTask;
        }
    }
}

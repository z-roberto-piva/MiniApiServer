using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSum;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Tests.UseCases.ProcessSum;

public sealed class ProcessSumUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateSummAndNotifyCoordinator()
    {
        var dataIn = DataIn.Create("input", 10, 3);
        var dataInRepository = new SingleDataInRepository(dataIn);
        var summRepository = new RecordingDataSummRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessSumUseCase(dataInRepository, summRepository, coordinator);

        var result = await useCase.ExecuteAsync(new ProcessSumCommand(dataIn.Id));

        Assert.Equal(13, result.Result);
        Assert.Single(summRepository.Items);
        Assert.Equal(dataIn.Id, coordinator.StartedIds.Single());
        Assert.Equal(dataIn.Id, coordinator.CompletedIds.Single());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowWhenInputDoesNotExist()
    {
        var dataInRepository = new SingleDataInRepository(null);
        var summRepository = new RecordingDataSummRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessSumUseCase(dataInRepository, summRepository, coordinator);

        var action = () => useCase.ExecuteAsync(new ProcessSumCommand(Guid.NewGuid()));

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

    private sealed class RecordingDataSummRepository : IDataSummRepository
    {
        public List<DataSumm> Items { get; } = [];

        public Task AddAsync(DataSumm dataSumm, CancellationToken cancellationToken = default)
        {
            Items.Add(dataSumm);
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

using MiniApiServer.Application.Abstractions.Services;
using MiniApiServer.Application.Common;
using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessDivision;
using MiniApiServer.Domain.Abstractions.Repositories;
using MiniApiServer.Domain.Entities;

namespace MiniApiServer.Application.Tests.UseCases.ProcessDivision;

public sealed class ProcessDivisionUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateDivisionAndNotifyCoordinator()
    {
        var dataIn = DataIn.Create("input", 12, 3);
        var dataInRepository = new SingleDataInRepository(dataIn);
        var divisionRepository = new RecordingDataDivisionRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessDivisionUseCase(dataInRepository, divisionRepository, coordinator);

        var result = await useCase.ExecuteAsync(new ProcessDivisionCommand(dataIn.Id));

        Assert.Equal(4, result.Result);
        Assert.Single(divisionRepository.Items);
        Assert.Equal(dataIn.Id, coordinator.StartedIds.Single());
        Assert.Equal(dataIn.Id, coordinator.CompletedIds.Single());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStoreZeroWhenDivisorIsZero()
    {
        var dataIn = DataIn.Create("input", 12, 0);
        var dataInRepository = new SingleDataInRepository(dataIn);
        var divisionRepository = new RecordingDataDivisionRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessDivisionUseCase(dataInRepository, divisionRepository, coordinator);

        var result = await useCase.ExecuteAsync(new ProcessDivisionCommand(dataIn.Id));

        Assert.Equal(0, result.Result);
        Assert.Equal(0, divisionRepository.Items.Single().Result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowWhenInputDoesNotExist()
    {
        var dataInRepository = new SingleDataInRepository(null);
        var divisionRepository = new RecordingDataDivisionRepository();
        var coordinator = new RecordingDataInStatusCoordinator();
        var useCase = new ProcessDivisionUseCase(dataInRepository, divisionRepository, coordinator);

        var action = () => useCase.ExecuteAsync(new ProcessDivisionCommand(Guid.NewGuid()));

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

    private sealed class RecordingDataDivisionRepository : IDataDivisionRepository
    {
        public List<DataDivision> Items { get; } = [];

        public Task AddAsync(DataDivision dataDivision, CancellationToken cancellationToken = default)
        {
            Items.Add(dataDivision);
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

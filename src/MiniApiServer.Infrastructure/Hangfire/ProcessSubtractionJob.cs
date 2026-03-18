using MiniApiServer.Application.Contracts;
using MiniApiServer.Application.UseCases.ProcessSubtraction;

namespace MiniApiServer.Infrastructure.Hangfire;

public sealed class ProcessSubtractionJob(ProcessSubtractionUseCase useCase)
{
    public Task ExecuteAsync(Guid dataInId)
        => useCase.ExecuteAsync(new ProcessSubtractionCommand(dataInId));
}
